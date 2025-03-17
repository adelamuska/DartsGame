using DartsGame.Entities;

namespace DartsGame.Interfaces.ServiceInterfaces.Statistics
{
    public interface IPlayerStatsService
    {
        Task<PlayerStats> GetPlayerStats(Guid playerId, int recentLegsLimit = 10);
    }
}
