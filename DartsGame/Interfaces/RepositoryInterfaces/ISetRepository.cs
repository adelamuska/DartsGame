using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface ISetRepository
    {
        Task<Set> GetSetById(Guid setId);
        Task<Set> GetSetWithLegs(Guid setId);
        Task<int> GetLastSetNumber(Guid matchId);
    }
}
