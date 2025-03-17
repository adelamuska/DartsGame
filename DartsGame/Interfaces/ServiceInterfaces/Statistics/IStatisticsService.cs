using System.Threading.Tasks;

namespace DartsGame.Interfaces.ServiceInterfaces.Statistics
{
    public interface IStatisticsService
    {
        Task SaveLegStatistics(Guid legId, Guid playerId);
        Task SaveSetStatistics(Guid setId, Guid playerId);
        Task SaveMatchStatistics(Guid matchId, Guid playerId);
    }
}
