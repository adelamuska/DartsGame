using DartsGame.DTOs;
using DartsGame.Entities;

namespace DartsGame.Repositories.Statistics
{
    public interface IPlayerStatsRepository
    {
        Task<Player> GetPlayerByIdAsync(Guid playerId);
        Task<IEnumerable<RecentLegStatDto>> GetRecentLegStats(Guid playerId, int limit);
    }

}
