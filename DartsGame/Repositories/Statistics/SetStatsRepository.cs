using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DartsGame.Repositories
{
    public class SetStatsRepository 
    {
        private readonly AppDbContext _context;

        public SetStatsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Leg>> GetLegsBySetId(Guid setId)
        {
            return await _context.Legs
                .Where(l => l.SetId == setId)
                .ToListAsync();
        }

        public async Task<int> GetLegsWon(Guid setId, Guid playerId)
        {
            var set = await _context.Sets
                .Where(s => s.SetId == setId)
                .Include(s => s.SetResults) 
                .FirstOrDefaultAsync();

            if (set == null)
                throw new KeyNotFoundException("Set not found.");

            return set.SetResults
                .Where(s => s.PlayerId == playerId)
                .Sum(s => s.LegsWon);
        }

        //public async Task<int> GetSixtyPlusCount(Guid setId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score >= 60 && t.Score <100 && t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId && t.Score >= 60 &&
        //                    !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        //public async Task<int> GetHundredPlusCount(Guid setId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score >= 100 && t.Score< 140 && t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId && t.Score >= 100 &&
        //                    !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        //public async Task<int> GetHundredFortyPlusCount(Guid setId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score >= 140 && t.Score < 180 && t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId  &&
        //                    !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        //public async Task<int> GetOneEightyCount(Guid setId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Score == 180 && t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId &&
        //                    !t.Turn.IsBusted)
        //        .CountAsync();
        //}

        //public async Task<int> GetTotalScore(Guid setId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
        //        .SumAsync(t => t.Score);
        //}

        //public async Task<int> GetDartsThrown(Guid setId, Guid playerId)
        //{
        //    return await _context.TurnThrows
        //        .Where(t => t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
        //        .SumAsync(t => (t.Throw1 > 0 ? 1 : 0) + (t.Throw2 > 0 ? 1 : 0) + (t.Throw3 > 0 ? 1 : 0));
        //}

        public async Task<IQueryable<TurnThrow>> GetFirstNineThrows(Guid setId, Guid playerId)
        {
            return _context.TurnThrows
                .Where(t => t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
                .OrderBy(t => t.Turn.TimeStamp)
                .Take(9);
        }

        public async Task<int> GetCheckoutAttempts(Guid setId, Guid playerId)
        {
            return await _context.Turns
                .Where(t => t.Leg.SetId == setId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutAttempt &&
                            !t.IsBusted)
                .GroupBy(t=> t.Leg.SetId)
                .CountAsync();
        }

        //public async Task<int> GetCheckoutSuccesses(Guid setId, Guid playerId)
        //{
        //    return await _context.Turns
        //        .Where(t => t.Leg.SetId == setId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutSuccess &&
        //                    !t.IsBusted)
        //        .GroupBy(t=> t.Leg.SetId)
        //        .CountAsync();
        //}

        //public async Task<int> GetCheckoutPoints(Guid setId, Guid playerId)
        //{
        //    return await _context.Turns
        //        .Where(t => t.Leg.SetId == setId && t.PlayerId == playerId && t.IsCheckoutSuccess &&
        //                    !t.IsBusted)
        //        .SelectMany(t => t.TurnThrows)
        //        .Where(tw => tw.Throw1.HasValue || tw.Throw2.HasValue || tw.Throw3.HasValue)
        //        .Select(tw => (tw.Throw1 ?? 0) + (tw.Throw2 ?? 0) + (tw.Throw3 ?? 0))
        //        .DefaultIfEmpty()
        //        .MaxAsync();
        //}

    }
}
