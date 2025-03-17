using DartsGame.DTO;

namespace DartsGame.Interfaces.ServiceInterfaces
{
    public interface ISetService
    {
        Task<SetDTO> GetById(Guid setId);
        Task DeleteSet(Guid setId);
    }
}
