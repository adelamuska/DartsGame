using DartsGame.DTO;

namespace DartsGame.Interfaces.ServiceInterfaces
{
    public interface ITurnThrowService
    {
        Task<TurnThrowDTO> GetById(Guid turnThrowId);
        Task DeleteTurnThrow(Guid turnThrowId);
    }
}
