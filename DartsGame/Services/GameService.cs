using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Helpers;
using DartsGame.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services
{
    public class GameService : IGameService
    {
        private readonly AppDbContext _context;
        public GameService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ProcessGameStateAfterTurn(Guid legId, int turnScore, string lastThrow)
        {
            var currentLeg = await _context.Legs.FindAsync(legId);
            if (currentLeg == null)
            {
                throw new InvalidOperationException("Leg not found.");
            }

            await ValidateLegCompletion(currentLeg, turnScore, lastThrow);

            await _context.Entry(currentLeg).ReloadAsync();

            if (currentLeg.IsFinished)
            {
                var set = await _context.Sets.FindAsync(currentLeg.SetId);

                await CheckSetCompletion(set.SetId);
            }
        }

        public async Task ValidateLegCompletion(Leg currentLeg, int turnScore, string lastThrow)
        {
            var playerScores = await _context.LegScores.Where(l => l.LegId == currentLeg.LegId).ToListAsync();
            var currentTurn = await _context.Turns
                .Where(l => l.LegId == currentLeg.LegId && !l.IsDeleted)
                .OrderByDescending(t => t.TimeStamp)
                .FirstOrDefaultAsync();

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

            int remainingScore = currentPlayerScore.RemainingScore - turnScore;

            if (DartsRulesHelper.IsBust(remainingScore, remainingScore == 0, lastThrow))
            {
                currentTurn.IsBusted = true;
                await _context.SaveChangesAsync();
                return;
            }
            else if (remainingScore == 0)
            {
                currentTurn.IsCheckoutSuccess = true;
                currentPlayerScore.RemainingScore = 0;
                currentLeg.IsFinished = true;
                await _context.SaveChangesAsync();

                await CompleteLeg(currentLeg, currentPlayerId);
            }
            else
            {
                currentPlayerScore.RemainingScore = remainingScore;

                bool canCheckout = DartsRulesHelper.CanCheckout(remainingScore);
                if (!canCheckout)
                {
                   
                        currentTurn.IsCheckoutAttempt = true;
                    
                }

                await _context.SaveChangesAsync();
            }
        }

        

        private async Task CompleteLeg(Leg leg, Guid winnerId)
        {
            leg.IsFinished = true;
            leg.WinnerId = winnerId;

            var setResult = await _context.SetResults.FirstOrDefaultAsync(s => s.SetId == leg.SetId && s.PlayerId == winnerId);

            if (setResult != null)
            {
                setResult.LegsWon++;
            }
            else
            {
                setResult = new SetResult
                {
                    SetId = leg.SetId,
                    PlayerId = winnerId,
                    LegsWon = 1
                };

                await _context.SetResults.AddAsync(setResult);
            }

            await _context.SaveChangesAsync();

            var set = await _context.Sets.FindAsync(leg.SetId);
            if (set != null)
            {
                await CheckSetCompletion(set.SetId);
            }
        }

        private async Task CheckSetCompletion(Guid setId)
        {
            var set = await _context.Sets.Include(s => s.Legs).FirstOrDefaultAsync(s => s.SetId == setId);

            if (set == null)
            {
                return;
            }

            var legsNeededToWin = (int)set.BestOfLegs ;
            var setResults = await _context.SetResults.Where(l => l.SetId == set.SetId).ToListAsync();

            if (setResults.Any(s => s.LegsWon >= legsNeededToWin))
            {
                await CompleteSet(set);
            }
            else
            {
                await CreateNewLegIfNeeded(setId);
            }
        }

        private async Task CompleteSet(Set set)
        {
            var setResults = await _context.SetResults.Where(l => l.SetId == set.SetId).ToListAsync();

            var legsNeededToWin = (int)set.BestOfLegs;
            var setWinner = setResults.FirstOrDefault(s => s.LegsWon >= legsNeededToWin);

            if (setWinner != null)
            {
                set.IsFinished = true;
                set.WinnerPlayerId = setWinner.PlayerId;

                await _context.SaveChangesAsync();

                bool matchCompleted = await CheckMatchCompleted(set.MatchId);

                if (!matchCompleted)
                {
                    var match = await _context.Matches.FindAsync(set.MatchId);
                    await CreateNewSetIfNeeded(match);
                }
            }
        }

        private async Task<bool> CheckMatchCompleted(Guid matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null) { return false; }

            var setsNeededToWin = (int)match.BestOfSets;

            var setsWonByPlayer = await _context.Sets
                .Where(s => s.MatchId == matchId && s.IsFinished && s.WinnerPlayerId.HasValue)
                .GroupBy(s => s.WinnerPlayerId.Value)
                .Select(g => new { PlayerId = g.Key, SetsWon = g.Count() })
                .ToListAsync();

            var matchWinner = setsWonByPlayer.FirstOrDefault(p => p.SetsWon >= setsNeededToWin);

            if (matchWinner != null)
            {
                if (!match.IsFinished)
                {
                    match.IsFinished = true;
                    match.WinnerPlayerId = matchWinner.PlayerId;
                    match.EndTime = DateTime.Now;

                    var winner = await _context.Players.FindAsync(matchWinner.PlayerId);
                    if (winner != null)
                    {
                        winner.GamesWon++;
                    }

                    var loserIds = await _context.PlayerMatches
                        .Where(m => m.MatchId == matchId && m.PlayerId != matchWinner.PlayerId)
                        .Select(m => m.PlayerId)
                        .ToListAsync();

                    foreach (var loserId in loserIds)
                    {
                        var loser = await _context.Players.FindAsync(loserId);
                        if (loser != null)
                        {
                            loser.GamesLost++;
                        }
                    }
                }

                    await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private async Task CreateNewLegIfNeeded(Guid setId)
        {
            var set = await _context.Sets.Include(s => s.Legs).FirstOrDefaultAsync(s => s.SetId == setId);

            if (set == null || set.IsFinished)
            {
                return;
            }

            if (set.Legs.Any(l => !l.IsFinished))
            {
                return;
            }

            var match = await _context.Matches.FindAsync(set.MatchId);
            var players = await _context.PlayerMatches
                .Where(m => m.MatchId == match.MatchId)
                .Select(m => m.PlayerId)
                .ToListAsync();

            var legsNeededToWin = (int)set.BestOfLegs;
            var setResults = await _context.SetResults.Where(s => s.SetId == setId).ToListAsync();

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
                return players.First(); 

            var lastStartingTurn = _context.Turns
                .Where(t => t.LegId == lastLeg.LegId && !t.IsDeleted)
                .OrderBy(t => t.TimeStamp)
                .FirstOrDefault();

            if (lastStartingTurn == null)
                return players.First();

            var lastStartingPlayerIndex = players.IndexOf(lastStartingTurn.PlayerId);
            return players[(lastStartingPlayerIndex + 1) % players.Count];
        }

        private async Task CreateNewSetIfNeeded(Match match)
        {
            if (match.IsFinished) { return; }

            var hasUnfinishedSets = await _context.Sets.AnyAsync(s => s.MatchId == match.MatchId && !s.IsFinished);

            if (hasUnfinishedSets)
            {
                return;
            }

            var players = await _context.PlayerMatches
                .Where(p => p.MatchId == match.MatchId)
                .Select(p => p.PlayerId)
                .ToListAsync();

            var lastSet = await _context.Sets
                .Where(s => s.MatchId == match.MatchId)
                .OrderByDescending(s => s.SetNumber)
                .FirstOrDefaultAsync();

            var newSetNumber = (lastSet?.SetNumber ?? 0) + 1;

           
            var newSet = new Set(Guid.NewGuid())
            {
                MatchId = match.MatchId,
                BestOfLegs = lastSet.BestOfLegs, 
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

            
            foreach (var playerId in players)
            {
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
