using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories.Statistics
{
    public class LegStatsRepository : ILegStatsRepository
    {
        protected readonly AppDbContext _context;

        public LegStatsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalScore(Guid legId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .SumAsync(s => s.Score);
        }

        //public async Task<int> GetDartsThrownCount(Guid legId, Guid playerId)
        //{
        //    var throws = await _context.TurnThrows
        //        .Where(t => t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted)
        //        .ToListAsync();

        //    int count = 0;
        //    foreach (var t in throws)
        //    {
        //        int turnCount = (t.Throw1.HasValue ? 1 : 0) +
        //                        (t.Throw2.HasValue && t.Throw1.HasValue ? 1 : 0) +
        //                        (t.Throw3.HasValue && t.Throw2.HasValue ? 1 : 0);
        //        count += turnCount;
        //    }
        //    return count;
        //}

        public async Task<int> GetDartsThrownCount(Guid legId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted)
                .SumAsync(t =>
                    (t.Throw1.HasValue ? 1 : 0) +
                    (t.Throw2.HasValue && t.Throw1.HasValue ? 1 : 0) +
                    (t.Throw3.HasValue && t.Throw2.HasValue ? 1 : 0)
                );
        }


        public async Task<int> GetSixtyPlusCount(Guid legId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Score >= 60 && t.Score < 100 && t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted &&
                            !t.Turn.IsBusted)
                .CountAsync();
        }

        public async Task<int> GetHundredPlusCount(Guid legId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Score >= 100 && t.Score < 140 && t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .CountAsync();
        }

        public async Task<int> GetHundredFortyPlusCount(Guid legId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Score >= 140 && t.Score < 180 && t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .CountAsync();
        }

        public async Task<int> GetOneEightyCount(Guid legId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Score == 180 && t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .CountAsync();
        }

        public async Task<int> GetCheckoutSuccessCount(Guid legId, Guid playerId)
        {
            return await _context.Turns
                .Where(t => t.LegId == legId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutSuccess &&
                            !t.IsBusted)
                .CountAsync();
        }

        public async Task<int> GetCheckoutAttemptCount(Guid legId, Guid playerId)
        {

            var checkoutAttempts = await _context.Turns
                .Where(t => t.LegId == legId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutAttempt &&
                            !t.IsBusted)
                //.GroupBy(t => t.LegId)
                .CountAsync();

            return checkoutAttempts;
        }

        public async Task<List<int>> GetFirstNineThrows(Guid legId, Guid playerId)
        {
            var firstThreeTurns = await _context.TurnThrows
                .Include(t => t.Turn)
                .Where(t => t.Turn.LegId == legId && t.Turn.PlayerId == playerId && !t.IsDeleted)
                .OrderBy(t => t.Turn.TimeStamp)
                .Take(3)
                .ToListAsync();

            var firstNineThrows = firstThreeTurns
                .SelectMany(t => new[]
                {
                    t.Turn.IsBusted ? 0 : t.Throw1 ?? 0,
                    t.Turn.IsBusted ? 0 : t.Throw2 ?? 0,
                    t.Turn.IsBusted ? 0 : t.Throw3 ?? 0
                })
                .Take(9)
                .ToList();

            return firstNineThrows;
        }



        public async Task<int> GetCheckoutPoints(Guid legId, Guid playerId)
        {
            return await _context.Turns
                .Where(t => t.LegId == legId && t.PlayerId == playerId && t.IsCheckoutSuccess &&
                            !t.IsBusted)
                .SelectMany(t => t.TurnThrows)
                .Where(tw => tw.Throw1.HasValue || tw.Throw2.HasValue || tw.Throw3.HasValue)
                //.SumAsync(tw => (tw.Throw1 ?? 0) + (tw.Throw2 ?? 0) + (tw.Throw3 ?? 0));
                .SumAsync(tw => tw.Score);
        }

        public async Task<int> GetSetsWon(Guid matchId, Guid playerId)
        {
            var match = await _context.Matches
                .Where(m => m.MatchId == matchId)
                .Include(m => m.Sets)
                .FirstOrDefaultAsync();

            if (match == null) { throw new Exception("Match not found."); }

            return match.Sets.Count(s => s.WinnerPlayerId == playerId);
        }

        public async Task<int> GetTotalScoreForMatch(Guid matchId, Guid playerId)
        {
            return await _context.TurnThrows
                .Where(t => t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .SumAsync(t => t.Score);
        }

    }
}
