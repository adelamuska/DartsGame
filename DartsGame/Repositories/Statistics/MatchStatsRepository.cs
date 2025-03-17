using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories.Statistics
{
    public class MatchStatsRepository : IMatchStatsRepository
    {
        private readonly AppDbContext _context;

        public MatchStatsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Set>> GetSetsByMatchId(Guid matchId)
        {
            return await _context.Sets
              .Where(s => s.MatchId == matchId)
              .OrderBy(s => s.SetNumber)
              .Include(s => s.Legs)
              .ToListAsync();
        }

        //public async Task<List<Leg>> GetLegsByMatchId(Guid matchId)
        //{
        //    return await _context.Legs
        //        .Where(l => l.Set.MatchId == matchId && !l.IsDeleted)
        //        .OrderBy(l => l.LegId)
        //        .ToListAsync();
        //}

        public async Task<int> GetSetsWon(Guid matchId, Guid playerId)
        {
            return await _context.Sets
                .Where(s => s.MatchId == matchId && s.WinnerPlayerId == playerId && !s.IsDeleted)
                .CountAsync();
        }


        public async Task<int> GetTotalDartsThrown(Guid setId, Guid playerId)
        {
            return await _context.TurnThrows
                //.Where(t => t.Turn.Leg.Set.SetId == setId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .Where(t => t.Turn.Leg.Set.SetId == setId && t.Turn.PlayerId == playerId && !t.IsDeleted)
                .SumAsync(t => (t.Throw1.HasValue ? 1 : 0) + (t.Throw2.HasValue ? 1 : 0) + (t.Throw3.HasValue ? 1 : 0));
        }

        //public async Task<int> GetCheckoutSuccesses(Guid matchId, Guid playerId)
        //{
        //    return await _context.Turns
        //        .Where(t => t.Leg.Set.MatchId == matchId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutSuccess && !t.IsBusted)
        //        .GroupBy(t => t.Leg.Set.MatchId)
        //        .CountAsync();
        //}

        //public async Task<int> GetCheckoutAttempts(Guid matchId, Guid playerId)
        //{
        //    return await _context.Turns
        //        .Where(t => t.Leg.Set.MatchId == matchId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutAttempt && !t.IsBusted)
        //        .GroupBy(t => t.Leg.Set.MatchId)
        //        .CountAsync();

        //}



        //public async Task<IQueryable<TurnThrow>> GetFirstNineThrows(Guid matchId, Guid playerId)
        //{
        //    return _context.TurnThrows
        //        .Where(t => t.Turn.Leg.Set.MatchId == matchId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
        //        .OrderBy(t => t.Turn.TimeStamp)
        //        .Take(9);
        //}




    }
}

