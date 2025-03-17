using DartsGame.DTOs;
using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces.Statistics
{
    public interface IPlayerStatsRepository
    {
        Task<Player> GetPlayerByIdAsync(Guid playerId);
        Task<IEnumerable<RecentLegStatDto>> GetRecentLegStats(Guid playerId, int limit);
    }

}
