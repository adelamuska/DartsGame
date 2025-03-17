using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface IPlayerRepository
    {
        Task<Player> GetById(Guid playerId);
        Task<List<Guid>> GetActivePlayerIds(Guid matchId);
    }
}
