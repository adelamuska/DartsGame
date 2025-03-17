using DartsGame.DTO;
using DartsGame.Repositories;

namespace DartsGame.Interfaces.ServiceInterfaces
{
    public interface ILegService
    {
        Task<LegDTO> GetById(Guid legId);
        Task DeleteLeg(Guid legId);
       

    }
}
