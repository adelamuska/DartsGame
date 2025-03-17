using DartsGame.DTO;

namespace DartsGame.Interfaces.ServiceInterfaces
{
    public interface IPlayerService
    {
        Task<PlayerDTO> GetById(Guid playerId);
        Task DeletePlayer(Guid playerId);


    }
}
