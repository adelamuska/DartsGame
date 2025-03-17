using DartsGame.DTOs;

namespace DartsGame.Interfaces.ServiceInterfaces.Statistics
{
    public interface IGameHistoryService
    {
        Task<List<MatchStatsDTO>> GetMatchStatsByMatchId(Guid matchId);
    }
}
