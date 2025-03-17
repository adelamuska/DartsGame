using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface ITurnRepository
    {
        Task<Turn> GetLatestTurnByLegId(Guid legId);
        Task<Turn> GetFirstTurnByLegId(Guid legId);
        Task<Turn> GetCurrentTurn(Guid legId);
    }
}
