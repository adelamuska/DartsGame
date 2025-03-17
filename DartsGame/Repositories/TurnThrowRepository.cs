using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class TurnThrowRepository : BaseRepository<TurnThrow>, ITurnThrowRepository
    {
        public TurnThrowRepository(AppDbContext context) : base(context) { }

        public async Task<List<TurnThrow>> GetTurnThrowsByTurnId(Guid turnId)
        {
            return await _context.TurnThrows
                .Where(t => t.TurnId == turnId)
               // .OrderBy(t => t.TurnThrowId)
                .ToListAsync();
        }



        //public async Task<TurnThrow> GetLastTurnThrow()
        //{
        //    return await _context.TurnThrows
        //        .OrderBy(t => t.TurnThrowId)
        //        .LastOrDefaultAsync();
        //}
    }
}