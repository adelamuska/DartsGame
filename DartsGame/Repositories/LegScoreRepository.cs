using DartsGame.Data;
using DartsGame.Entities;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Repositories
{
    public class LegScoreRepository : BaseRepository<LegScore>
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

        //public async Task UpdatePlayerRemainingScore(Guid legId, Guid playerId, int remainingScore)
        //{
        //    var playerScore = await _legRepository.GetPlayerLegScore(legId, playerId);

        //    if (playerScore != null)
        //    {
        //        playerScore.RemainingScore = remainingScore;
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}