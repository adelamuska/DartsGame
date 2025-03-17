using System.Text.RegularExpressions;
using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class TurnRepository : BaseRepository<Turn>, ITurnRepository
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

      

        public async Task<Turn> GetCurrentTurn(Guid legId)
        {
            return await _context.Turns
                .Where(t => t.LegId == legId && !t.IsDeleted)
                .OrderByDescending(t => t.TimeStamp)
                .FirstOrDefaultAsync();
        }

    }
}