using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class MatchRepository : BaseRepository<Match>
    {
        public MatchRepository(AppDbContext context) : base(context) { }

        public async Task<Match> GetById(Guid matchId)
        {
            return await _context.Matches.Include(m => m.Sets).ThenInclude(s => s.Legs) .FirstOrDefaultAsync(m => m.MatchId == matchId);
        }
    }
}
