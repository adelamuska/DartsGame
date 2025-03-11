using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class LegRepository : BaseRepository<Leg>
    {
        public LegRepository(AppDbContext context) : base(context) { }

        public async Task<Leg> GetLegById(Guid legId)
        {
            return await _context.Legs.FindAsync(legId);
        }

        public async Task<Leg> GetLegWithScores(Guid legId)
        {
            return await _context.Legs
                .Include(l => l.LegScores)
                .FirstOrDefaultAsync(l => l.LegId == legId);
        }

        public async Task<List<LegScore>> GetLegScores(Guid legId)
        {
            return await _context.LegScores.Where(l => l.LegId == legId).ToListAsync();
        }

        public async Task<Leg> GetNextUnfinishedLeg(Guid setId)
        {
            return await _context.Legs
                .Where(l => l.SetId == setId && !l.IsFinished)
                .OrderBy(l => l.LegNumber)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetLastLegNumber(Guid setId)
        {
            return await _context.Legs
                .Where(l => l.SetId == setId)
                .Select(l => l.LegNumber)
                .MaxAsync();
        }

        public async Task<bool> HasUnfinishedLegs(Guid setId)
        {
            return await _context.Legs.AnyAsync(l => l.SetId == setId && !l.IsFinished);
        }

        public async Task<Leg> GetCurrentLeg(Guid matchId)
        {
            return await _context.Legs
                .Where(l => l.SetId == matchId && !l.IsFinished)
                .OrderByDescending(l => l.LegNumber)
                .FirstOrDefaultAsync();
        }
    }
}