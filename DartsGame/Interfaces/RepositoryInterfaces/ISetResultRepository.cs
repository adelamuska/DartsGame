using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface ISetResultRepository
    {
        Task<List<SetResult>> GetSetResultsBySetId(Guid setId);
        Task<SetResult> GetPlayerSetResult(Guid setId, Guid playerId);
    }
}
