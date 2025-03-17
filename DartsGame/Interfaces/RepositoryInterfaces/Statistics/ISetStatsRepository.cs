using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces.Statistics
{
    public interface ISetStatsRepository
    {
        Task<List<Leg>> GetLegsBySetId(Guid setId);
        Task<int> GetLegsWon(Guid setId, Guid playerId);
    }
}
