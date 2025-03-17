using DartsGame.Entities;

namespace DartsGame.Interfaces.RepositoryInterfaces
{
    public interface ILegRepository
    {
        Task<Leg> GetCurrentLeg(Guid matchId);

    }
}
