using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class LegScoreRepository : BaseRepository<LegScore>, ILegScoreRepository
    {
        public LegScoreRepository(AppDbContext context) : base(context) { }

        public async Task<List<LegScore>> GetLegScoresByLegId(Guid legId)
        {
            return await _context.LegScores
                .Where(l => l.LegId == legId)
                .ToListAsync();
        }

        public async Task<LegScore> GetPlayerLegScore(Guid legId, Guid playerId)
        {
            return await _context.LegScores
                .FirstOrDefaultAsync(l => l.LegId == legId && l.PlayerId == playerId);
        }

    }
}