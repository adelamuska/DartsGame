using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class TurnThrowRepository : BaseRepository<TurnThrow>
    {
        public TurnThrowRepository(AppDbContext context) : base(context) { }

        public async Task<List<TurnThrow>> GetTurnThrowsByTurnId(Guid turnId)
        {
            return await _context.TurnThrows
                .Where(t => t.TurnId == turnId)
                .OrderBy(t => t.TurnThrowId)
                .ToListAsync();
        }

        //public async Task ClearRemainingThrows(Guid turnId)
        //{
        //    var turnThrows = await _context.TurnThrows
        //        .Where(t => t.TurnId == turnId)
        //        .OrderBy(t => t.TurnThrowId)
        //        .ToListAsync();

        //    if (turnThrows.Count == 1 && turnThrows[0].Throw1.HasValue)
        //    {
        //        turnThrows[0].Throw2 = null;
        //        turnThrows[0].Throw3 = null;
        //    }
        //    else if (turnThrows.Count == 2 && turnThrows[1].Throw2.HasValue)
        //    {
        //        turnThrows[1].Throw3 = null;
        //    }

        //    await _context.SaveChangesAsync();
        //}


        public async Task<TurnThrow> GetLastTurnThrow()
        {
            return await _context.TurnThrows
                .OrderBy(t => t.TurnThrowId)
                .LastOrDefaultAsync();
        }
    }
}