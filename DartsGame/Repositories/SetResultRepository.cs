using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class SetResultRepository : BaseRepository<SetResult>
    {
        public SetResultRepository(AppDbContext context) : base(context) { }

        public async Task<List<SetResult>> GetSetResultsBySetId(Guid setId)
        {
            return await _context.SetResults
                .Where(s => s.SetId == setId)
                .ToListAsync();
        }

        public async Task<SetResult> GetPlayerSetResult(Guid setId, Guid playerId)
        {
            return await _context.SetResults
                .FirstOrDefaultAsync(s => s.SetId == setId && s.PlayerId == playerId);
        }

        //public async Task IncrementPlayerLegsWon(Guid setId, Guid playerId)
        //{
        //    var setResult = await _context.SetResults
        //        .FirstOrDefaultAsync(s => s.SetId == setId && s.PlayerId == playerId);

        //    if (setResult != null)
        //    {
        //        setResult.LegsWon++;
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}