using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace DartsGame.Services
{
    public class GameService
    {
        private readonly AppDbContext _context;
        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ValidateLegCompletion(Leg currentLeg, int turnScore, string LastThrow)
        {
            // Get player's current score before this turn
            var playerScores = await _context.LegScores.Where(l => l.LegId == currentLeg.LegId).ToListAsync();
            var currentTurn = await _context.Turns.Where(l => l.LegId == currentLeg.LegId && !l.IsDeleted).OrderByDescending(t => t.TimeStamp).FirstOrDefaultAsync();

            if (currentTurn == null)
            {
                throw new InvalidOperationException("Current turn not found.");
            }

            var currentPlayerId = currentTurn.PlayerId;
            var currentPlayerScore = playerScores.FirstOrDefault(p => p.PlayerId == currentPlayerId);

            if (currentPlayerScore == null)
            {
                throw new InvalidOperationException("Player score not found.");
            }

            // Calculate new score after this turn
            int remainingScore = currentPlayerScore.RemainingScore - turnScore;

            // Check for bust (score < 0 or score == 1 or ending without a double)
            if (IsBust(remainingScore, remainingScore == 0, LastThrow))
            {

                // Bust: Score remains the same, turn is marked as busted
                currentTurn.IsBusted = true;
                await _context.SaveChangesAsync();
            }

            else if (remainingScore == 0)
            {

                currentTurn.IsCheckoutSuccess = true;
                await _context.SaveChangesAsync();

                await CompleteLeg(currentLeg, currentPlayerId);
                await CheckSetCompletion(currentLeg.SetId);
            }
            else
            {
                currentPlayerScore.RemainingScore = remainingScore;

                bool canCheckout = CanCheckOutNextTurn(remainingScore);
                if (canCheckout)
                {

                    var nextTurn = await _context.Turns.Where(l => l.LegId == currentLeg.LegId && !l.IsDeleted).OrderByDescending(t => t.TimeStamp).Skip(1).FirstOrDefaultAsync();

                    if (nextTurn != null)
                    {
                        nextTurn.IsCheckoutAttempt = true;
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        private bool IsBust(int remainingScore, bool IsZero, string lastThrow)
        {

            if (remainingScore < 0)
            {
                return true;
            }

            if (remainingScore == 1)
            {
                return true;
            }

            if (IsZero && !IsValidCheckout(lastThrow))
            {
                return true;
            }

            return false;
        }

        private bool IsValidCheckout(string lastThrow)
        {

            return lastThrow != null && lastThrow.StartsWith("D");
        }

        private bool CanCheckOutNextTurn(int score)
        {

            if (score <= 170 && score != 169 && score != 168 && score != 166 &&
                 score != 165 && score != 163 && score != 162 && score != 159)
            {
                return true;
            }
            return false;
        }

        private async Task CompleteLeg(Leg leg, Guid WinnerId)
        {
            leg.IsFinished = true;
            leg.WinnerId = WinnerId;

            // Update set results for the winner
            var setResult = await _context.SetResults.FirstOrDefaultAsync(s => s.SetId == leg.SetId && s.PlayerId == WinnerId);

            if (setResult != null)
            {
                setResult.LegsWon++;
            }
            else
            {
                setResult = new SetResult
                {
                    SetId = leg.SetId,
                    PlayerId = WinnerId,
                    LegsWon = 1
                };

                await _context.SetResults.AddAsync(setResult);
            }
        }
        public async Task CreateNewLegIfNeeded(Guid setId)
        {
            var set = await _context.Sets.Include(s => s.Legs).FirstOrDefaultAsync(s => s.SetId == setId);

            if (set == null || set.IsFinished)
            {
                return;
            }

            var match = await _context.Matches.FindAsync(set.MatchId);
            var players = await _context.PlayerMatches.Where(m => m.MatchId == match.MatchId).Select(m => m.PlayerId).ToListAsync();


            // Check if we need a new leg or if the set is complete
            var legsNeededToWin = (int)Math.Ceiling((double)set.BestOfLegs / 2);
            var setResults = await _context.SetResults.Where(s => s.SetId == setId).ToListAsync();

            // Check if any player has won enough legs to win the set
            if (setResults.Any(s => s.LegsWon >= legsNeededToWin))
            {
                await CompleteSet(set);
            }
            else
            {
                var startingPlayer = DetermineNextStartingPlayer(set, players);
                var lastLegNumber = set.Legs.Max(l => l.LegNumber);

                var newLeg = new Leg(
                    Guid.NewGuid(),
                    setId,
                    lastLegNumber + 1,
                    false
                    );

                _context.Legs.Add(newLeg);

                foreach (var playerId in players)
                {
                    var legScore = new LegScore
                    {
                        LegId = newLeg.LegId,
                        PlayerId = playerId,
                        RemainingScore = (int)match.StartingScore
                    };

                    _context.LegScores.Add(legScore);
                }

                var initialTurn = new Turn(
                    Guid.NewGuid(),
                    startingPlayer,
                    newLeg.LegId,
                    DateTime.UtcNow,
                    false,
                    false,
                    false
                    );

                _context.Turns.Add(initialTurn);

                await _context.SaveChangesAsync();


            }
        }

        private Guid DetermineNextStartingPlayer(Set set, List<Guid> players)
        {
            var lastLeg = set.Legs.OrderByDescending(l => l.LegNumber).FirstOrDefault();
            if (lastLeg == null)
            {

                return players.First();
            }

            var lastStartingTurn = _context.Turns.Where(l => l.LegId == lastLeg.LegId && !l.IsDeleted).OrderByDescending(t => t.TimeStamp).FirstOrDefault();
            if (lastStartingTurn == null)
            {
                return players.First();
            }

            var lastStartingPlayerIndex = players.IndexOf(lastStartingTurn.PlayerId);
            return players[(lastStartingPlayerIndex + 1) % players.Count];

        }

        private async Task CompleteSet(Set set)
        {
            var setResults = await _context.SetResults.Where(l => l.SetId == set.SetId).ToListAsync();

            var legsNeededToWin = (int)Math.Ceiling((double)set.BestOfLegs / 2);
            var setWinner = setResults.FirstOrDefault(s => s.LegsWon >= legsNeededToWin);

            if (setWinner != null)
            {
                set.IsFinished = true;
                set.WinnerPlayerId = setWinner.PlayerId;

                await _context.SaveChangesAsync();

                await CheckMatchCompleted(set.MatchId);
            }
        }

        private async Task CheckSetCompletion(Guid setId)
        {
            var set = await _context.Sets.Include(s => s.SetId == setId).FirstOrDefaultAsync();

            if (set == null)
            {
                return;
            }

            var legsNeededToWin = (int)Math.Ceiling((double)set.BestOfLegs / 2);
            var setResults = await _context.SetResults.Where(l => l.SetId == set.SetId).ToListAsync();

            if (setResults.Any(s => s.LegsWon >= legsNeededToWin))
            {
                await CompleteSet(set);
            }
            else
            {
                // Create new leg if set is not complete
                await CreateNewLegIfNeeded(setId);
            }
        }

        private async Task CheckMatchCompleted(Guid matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null) { return; }

            var setsNeededToWin = (int)Math.Ceiling((double)((int)match.BestOfSets) / 2);

            var setsWonByPlayer = await _context.Sets.Where(s => s.MatchId == matchId && s.IsFinished && s.WinnerPlayerId.HasValue).GroupBy(s => s.WinnerPlayerId.Value)
                .Select(g => new { PlayerId = g.Key, SetsWon = g.Count() }).ToListAsync();

            var matchWinner = setsWonByPlayer.FirstOrDefault(p => p.SetsWon >= setsNeededToWin);

            if (matchWinner != null)
            {

                match.IsFinished = true;
                match.WinnerPlayerId = matchWinner.PlayerId;
                match.EndTime = DateTime.Now;

                var winner = await _context.Players.FindAsync(matchWinner.PlayerId);
                if (winner != null)
                {
                    winner.GamesWon++;
                }

                var loserIds = await _context.PlayerMatches.Where(m => m.MatchId == matchId && m.PlayerId != matchWinner.PlayerId).ToListAsync();

                foreach (var loserId in loserIds)
                {
                    var loser = await _context.Players.FindAsync(loserId);
                    if (loser != null)
                    {
                        loser.GamesLost++;
                    }
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                await CreateNewSetIfNeeded(match);
            }
        }

        private async Task CreateNewSetIfNeeded(Match match)
        {
            if (match.IsFinished) { return; }

            var players = await _context.PlayerMatches.Where(p=> p.MatchId == match.MatchId).Select(p => p.PlayerId).ToListAsync();
            var lastSet = await _context.Sets.Where(s=> s.MatchId == match.MatchId).OrderByDescending(s=> s.SetNumber).FirstOrDefaultAsync();

            var newSetNumber = (lastSet?.SetNumber ?? 0) + 1;

            var newSet = new Set(Guid.NewGuid())
            {
                MatchId = match.MatchId,
                BestOfLegs = 3,//modifikoje
                SetNumber = newSetNumber,
                IsFinished = false,
            };

            _context.Sets.Add(newSet);
            await _context.SaveChangesAsync();

            var firstLeg = new Leg(
                 Guid.NewGuid(),
                newSet.SetId,
                1, 
                false
            );
                _context.Legs.Add(firstLeg);

            foreach (var playerId in players) {

                var legScore = new LegScore
                {
                    LegId = firstLeg.LegId,
                    PlayerId = playerId,
                    RemainingScore = (int)match.StartingScore
                };

                _context.LegScores.Add(legScore);
            }

            var initialTurn = new Turn(
                Guid.NewGuid(),
                players.First(), 
                firstLeg.LegId,
                DateTime.UtcNow,
                false,
                false,
                false
            );
            _context.Turns.Add(initialTurn);

            await _context.SaveChangesAsync();
        }

    }
}
