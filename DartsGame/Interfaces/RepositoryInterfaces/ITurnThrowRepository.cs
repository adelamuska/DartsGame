using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface ITurnThrowRepository
    {
        Task<List<TurnThrow>> GetTurnThrowsByTurnId(Guid turnId);
    }
}
