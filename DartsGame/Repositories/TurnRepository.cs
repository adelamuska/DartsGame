using System.Text.RegularExpressions;
using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class TurnRepository : BaseRepository<Turn>
    {
        public TurnRepository(AppDbContext context) : base(context) { }

        public async Task<Turn> GetLatestTurnByLegId(Guid legId)
        {
            return await _context.Turns
                .Where(t => t.LegId == legId && !t.IsDeleted)
                .OrderByDescending(t => t.TimeStamp)
                .FirstOrDefaultAsync();
        }

        public async Task<Turn> GetFirstTurnByLegId(Guid legId)
        {
            return await _context.Turns
                .Where(t => t.LegId == legId && !t.IsDeleted)
                .OrderBy(t => t.TimeStamp)
                .FirstOrDefaultAsync();
        }

        //public async Task MarkTurnAsBusted(Guid turnId)
        //{
        //    var turn = await _context.Turns.FindAsync(turnId);
        //    if (turn != null)
        //    {
        //        turn.IsBusted = true;
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task MarkTurnAsCheckoutSuccess(Guid turnId)
        //{
        //    var turn = await _context.Turns.FindAsync(turnId);
        //    if (turn != null)
        //    {
        //        turn.IsCheckoutSuccess = true;
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public async Task MarkTurnAsCheckoutAttempt(Guid turnId)
        //{
        //    var turn = await _context.Turns.FindAsync(turnId);
        //    if (turn != null)
        //    {
        //        turn.IsCheckoutAttempt = true;
        //        await _context.SaveChangesAsync();
        //    }
        //}

        public async Task<Turn> GetCurrentTurn(Guid legId)
        {
            return await _context.Turns
                .Where(t => t.LegId == legId && !t.IsDeleted)
                .OrderByDescending(t => t.TimeStamp)
                .FirstOrDefaultAsync();
        }

    }
}