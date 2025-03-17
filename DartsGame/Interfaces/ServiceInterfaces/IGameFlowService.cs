using DartsGame.Entities;

namespace DartsGame.Interfaces.ServiceInterfaces
{
    public interface IGameFlowService
    {

        Task ProcessGameStateAfterTurn(Guid legId, int turnScore, string lastThrow);
        Task ValidateLegCompletion(Leg currentLeg, int turnScore, string lastThrow);



    }
}
