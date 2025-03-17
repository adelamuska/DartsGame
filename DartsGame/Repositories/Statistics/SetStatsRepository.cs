using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces.Statistics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DartsGame.Repositories
{
    public class SetStatsRepository : ISetStatsRepository
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

        //public async Task<int> GetLegsWon(Guid setId, Guid playerId)
        //{
        //    var set = await _context.Sets
        //        .Where(s => s.SetId == setId)
        //        .Include(s => s.SetResults) 
        //        .FirstOrDefaultAsync();

        //    if (set == null)
        //        throw new KeyNotFoundException("Set not found.");

        //    return set.SetResults
        //        .Where(s => s.PlayerId == playerId)
        //        .Sum(s => s.LegsWon);
        //}

        public async Task<int> GetLegsWon(Guid setId, Guid playerId)
        {
            return await _context.SetResults
                .Where(sr => sr.SetId == setId && sr.PlayerId == playerId)
                .SumAsync(sr => sr.LegsWon);
        }

        //public async Task<IQueryable<TurnThrow>> GetFirstNineThrows(Guid setId, Guid playerId)
        //{
        //    return _context.TurnThrows
        //        .Where(t => t.Turn.Leg.SetId == setId && t.Turn.PlayerId == playerId && !t.IsDeleted && !t.Turn.IsBusted)
        //        .OrderBy(t => t.Turn.TimeStamp)
        //        .Take(9);
        //}

        //public async Task<int> GetCheckoutAttempts(Guid setId, Guid playerId)
        //{
        //    return await _context.Turns
        //        .Where(t => t.Leg.SetId == setId && t.PlayerId == playerId && !t.IsDeleted && t.IsCheckoutAttempt &&
        //                    !t.IsBusted)
        //        .GroupBy(t=> t.Leg.SetId)
        //        .CountAsync();
        //}


    }
}
