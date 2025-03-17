using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Helpers;
using DartsGame.Interfaces.RepositoryInterfaces;
using DartsGame.Interfaces.ServiceInterfaces;
using DartsGame.Services.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services
{
    public class GameFlowService : IGameFlowService
    {
        private readonly AppDbContext _context;
        private readonly IGameRepositoryService _repository;
        private readonly StatisticsService _statisticService;



        public GameFlowService(AppDbContext context, IGameRepositoryService repository, StatisticsService statisticsService)
        {

            _context = context;
            _repository = repository;
            _statisticService = statisticsService;


        }

        public async Task ProcessGameStateAfterTurn(Guid legId, int turnScore, string lastThrow)
        {
            var currentLeg = await _repository.LegRepository.GetLegById(legId);
            if (currentLeg == null)
            {
                throw new InvalidOperationException("Leg not found.");
            }

            await ValidateLegCompletion(currentLeg, turnScore, lastThrow);

            await _context.Entry(currentLeg).ReloadAsync();

            if (currentLeg.IsFinished)
            {
                var set = await _repository.SetRepository.GetSetById(currentLeg.SetId);
                await CheckSetCompletion(set.SetId);
            }
        }

        public async Task ValidateLegCompletion(Leg currentLeg, int turnScore, string lastThrow)
        {
            var legScores = await _repository.LegScoreRepository.GetLegScoresByLegId(currentLeg.LegId);
            var currentTurn = await _repository.TurnRepository.GetLatestTurnByLegId(currentLeg.LegId);

            if (currentTurn == null)
            {
                throw new InvalidOperationException("Current turn not found.");
            }

            var currentPlayerId = currentTurn.PlayerId;
            var currentPlayerScore = legScores.FirstOrDefault(p => p.PlayerId == currentPlayerId);

            if (currentPlayerScore == null)
            {
                throw new InvalidOperationException("Player score not found.");
            }

            var lastTurnThrow = await _repository.TurnThrowRepository.GetTurnThrowsByTurnId(currentTurn.TurnId);

            int remainingScore = currentPlayerScore.RemainingScore - turnScore;

            if (DartsRulesHelper.IsBust(remainingScore, remainingScore == 0, lastThrow))
            {
                await MarkTurnAsBusted(currentTurn.TurnId);
                await MarkTurnAsCheckoutAttempt(currentTurn.TurnId);
                return;
            }
            else if (remainingScore == 0)
            {
                await CheckOut(currentLeg, currentTurn);
            }
            else
            {
                await UpdatePlayerRemainingScore(currentLeg.LegId, currentPlayerId, remainingScore);

                bool canCheckout = DartsRulesHelper.CanCheckout(remainingScore);
                if (!canCheckout)
                {
                    await MarkTurnAsCheckoutAttempt(currentTurn.TurnId);
                }
            }
        }

        public async Task MarkTurnAsBusted(Guid turnId)
        {
            var turn = await _context.Turns.FindAsync(turnId);
            if (turn != null)
            {
                turn.IsBusted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkTurnAsCheckoutSuccess(Guid turnId)
        {
            var turn = await _context.Turns.FindAsync(turnId);
            if (turn != null)
            {
                turn.IsCheckoutSuccess = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkTurnAsCheckoutAttempt(Guid turnId)
        {
            var turn = await _context.Turns.FindAsync(turnId);
            if (turn != null)
            {
                turn.IsCheckoutAttempt = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CheckOut(Leg currentLeg, Turn currentTurn)
        {
            var currentPlayerId = currentTurn.PlayerId;

            await MarkTurnAsCheckoutSuccess(currentTurn.TurnId);
            await UpdatePlayerRemainingScore(currentLeg.LegId, currentPlayerId, 0);

            currentLeg.IsFinished = true;
            await _context.SaveChangesAsync();

            await ClearRemainingThrows(currentTurn.TurnId);
            await CompleteLeg(currentLeg, currentPlayerId);
        }

        public async Task UpdatePlayerRemainingScore(Guid legId, Guid playerId, int remainingScore)
        {
            var playerScore = await _repository.LegScoreRepository.GetPlayerLegScore(legId, playerId);

            if (playerScore != null)
            {
                playerScore.RemainingScore = remainingScore;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearRemainingThrows(Guid turnId)
        {
            var turnThrows = await _repository.TurnThrowRepository.GetTurnThrowsByTurnId(turnId);

            if (turnThrows.Count == 1 && turnThrows[0].Throw1.HasValue && !turnThrows[0].Throw2.HasValue && !turnThrows[0].Throw3.HasValue)
            {
                turnThrows[0].Throw2 = null;
                turnThrows[0].Throw3 = null;
            }
            else if (turnThrows.Count == 2 && turnThrows[1].Throw1.HasValue && turnThrows[1].Throw2.HasValue && !turnThrows[1].Throw3.HasValue)
            {
                turnThrows[1].Throw3 = null;
            }

            await _context.SaveChangesAsync();
        }

        private async Task CompleteLeg(Leg leg, Guid winnerId)
        {
            leg.IsFinished = true;
            leg.WinnerId = winnerId;
            await _context.SaveChangesAsync();

            await IncrementPlayerLegsWon(leg.SetId, winnerId);

            var legScores = await _repository.LegScoreRepository.GetLegScoresByLegId(leg.LegId);

            foreach (var legScore in legScores)
            {
                await _statisticService.SaveLegStatistics(leg.LegId, legScore.PlayerId);
            }

            var set = await _repository.SetRepository.GetSetById(leg.SetId);
            if (set != null)
            {
                await CheckSetCompletion(set.SetId);
            }
        }

        public async Task IncrementPlayerLegsWon(Guid setId, Guid playerId)
        {
            var setResult = await _repository.SetResultRepository.GetPlayerSetResult(setId, playerId);

            if (setResult != null)
            {
                setResult.LegsWon++;
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckSetCompletion(Guid setId)
        {
            var set = await _repository.SetRepository.GetSetWithLegs(setId);
            if (set == null)
            {
                return;
            }

            var legsNeededToWin = (int)set.BestOfLegs;
            var setResults = await _repository.SetResultRepository.GetSetResultsBySetId(setId);

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
            if (set.IsFinished) return;

            var setResults = await _repository.SetResultRepository.GetSetResultsBySetId(set.SetId);

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
                    var match = await _repository.MatchRepository.GetById(set.MatchId);
                    await CreateNewSetIfNeeded(match);
                }
            }

            foreach (var setResult in setResults)
            {
                await _statisticService.SaveSetStatistics(set.SetId, setResult.PlayerId);
            }
        }

        private async Task CompleteMatch(Guid matchId, Guid winnerPlayerId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match != null && !match.IsFinished)
            {
                match.IsFinished = true;
                match.WinnerPlayerId = winnerPlayerId;
                match.EndTime = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            var playerIds = await _repository.MatchRepository.GetMatchPlayerIds(matchId);

            foreach (var playerId in playerIds)
            {
                await _statisticService.SaveMatchStatistics(matchId, playerId);
            }
        }

        private async Task<bool> CheckMatchCompleted(Guid matchId)
        {
            var match = await _repository.MatchRepository.GetById(matchId);
            if (match == null) { return false; }

            var setsNeededToWin = (int)match.BestOfSets;
            var setsWonByPlayer = await _repository.SetRepository.GetSetsWonByPlayers(matchId);

            var matchWinner = setsWonByPlayer.FirstOrDefault(p => p.Value >= setsNeededToWin);

            if (matchWinner.Key != default(Guid))
            {
                if (!match.IsFinished)
                {
                    await CompleteMatch(matchId, matchWinner.Key);

                    var loserIds = await _repository.MatchRepository.GetMatchPlayerIds(matchId);
                    loserIds.Remove(matchWinner.Key);

                    await UpdatePlayersStats(matchWinner.Key, loserIds);
                }

                return true;
            }

            return false;
        }

        public async Task UpdatePlayersStats(Guid winnerId, List<Guid> loserIds)
        {
            var winner = await _context.Players.FindAsync(winnerId);
            if (winner != null)
            {
                winner.GamesWon++;
            }

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

        private async Task CreateNewLegIfNeeded(Guid setId)
        {
            var set = await _repository.SetRepository.GetSetWithLegs(setId);

            if (set == null || set.IsFinished)
            {
                return;
            }

            if (await _repository.LegRepository.HasUnfinishedLegs(setId))
            {
                return;
            }

            var match = await _repository.MatchRepository.GetById(set.MatchId);
            var players = await _repository.MatchRepository.GetMatchPlayerIds(match.MatchId);

            var startingPlayer = await DetermineNextStartingPlayer(set, players);
            var lastLegNumber = await _repository.LegRepository.GetLastLegNumber(setId);

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

        private async Task<Guid> DetermineNextStartingPlayer(Set set, List<Guid> players)
        {
            var lastLeg = set.Legs.OrderByDescending(l => l.LegNumber).FirstOrDefault();
            if (lastLeg == null)
                return players.First();

            var lastLegStartingTurn = await _repository.TurnRepository.GetFirstTurnByLegId(lastLeg.LegId);

            if (lastLegStartingTurn == null)
                return players.First();

            var lastLegStartingPlayerIndex = players.IndexOf(lastLegStartingTurn.PlayerId);
            return players[(lastLegStartingPlayerIndex + 1) % players.Count];
        }

        private async Task CreateNewSetIfNeeded(Match match)
        {
            if (match.IsFinished) { return; }

            if (await _repository.SetRepository.HasUnfinishedSets(match.MatchId))
            {
                return;
            }

            var players = await _repository.MatchRepository.GetMatchPlayerIds(match.MatchId);
            var lastSetNumber = await _repository.SetRepository.GetLastSetNumber(match.MatchId);
            var lastSet = await _context.Sets
                .Where(s => s.MatchId == match.MatchId && s.SetNumber == lastSetNumber)
                .FirstOrDefaultAsync();

            var newSet = new Set()
            {
                SetId = Guid.NewGuid(),
                MatchId = match.MatchId,
                BestOfLegs = lastSet.BestOfLegs,
                SetNumber = lastSetNumber + 1,
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

                var setResult = new SetResult
                {
                    SetId = newSet.SetId,
                    PlayerId = playerId,
                    LegsWon = 0
                };
                 _context.SetResults.Add(setResult);

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

