using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface ILegScoreRepository
    {
        Task<List<LegScore>> GetLegScoresByLegId(Guid legId);
        Task<LegScore> GetPlayerLegScore(Guid legId, Guid playerId);
    }
}
