using DartsGame.Entities;

namespace DartsGame.Interfaces
{
    public interface IGameService
    {

        Task ProcessGameStateAfterTurn(Guid legId, int turnScore, string lastThrow);
        Task ValidateLegCompletion(Leg currentLeg, int turnScore, string lastThrow);



    }
}
