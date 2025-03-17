using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface IMatchRepository
    {
        Task<Match> GetById(Guid matchId);
        Task<List<Guid>> GetMatchPlayerIds(Guid matchId);
    }
}
