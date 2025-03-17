using DartsGame.DTOs;

namespace DartsGame.Interfaces.RepositoryInterfaces.Statistics
{
    public interface IGameHistoryRepository
    {
        Task<List<MatchStatsDTO>> GetMatchStatsByMatchId(Guid matchId);
    }
}
