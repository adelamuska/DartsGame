using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces.Statistics
{
    public interface IMatchStatsRepository
    {
        Task<List<Set>> GetSetsByMatchId(Guid matchId);
        Task<int> GetSetsWon(Guid matchId, Guid playerId);
        Task<int> GetTotalDartsThrown(Guid setId, Guid playerId);

    }
}
