using DartsGame.Repositories.Statistics;
using System.Linq;
using DartsGame.Entities;
using DartsGame.Interfaces.ServiceInterfaces;
using DartsGame.Interfaces.ServiceInterfaces.Statistics;

namespace DartsGame.Services.Statistics
{
    public class LegStatsService : ILegStatsService
    {
        private readonly LegStatsRepository _statsRepository;

        public LegStatsService(LegStatsRepository statsRepository)
        {
            _statsRepository = statsRepository;
        }

        public async Task<decimal> CalculatePPD(Guid legId, Guid playerId)
        {
            var totalScore = await _statsRepository.GetTotalScore(legId, playerId);
            var dartsThrown = await _statsRepository.GetDartsThrownCount(legId, playerId);
            return dartsThrown > 0 ? (decimal)totalScore / dartsThrown : 0;
        }

        public async Task<decimal> CalculateFirst9PPD(Guid legId, Guid playerId)
        {
            var firstNineThrows = await _statsRepository.GetFirstNineThrows(legId, playerId); 

            var totalScoreFirst9 = firstNineThrows.Sum(); 
            var dartsThrown = await _statsRepository.GetDartsThrownCount(legId, playerId); 

            if (dartsThrown > 0 && dartsThrown < 9)
            {
                return (decimal)totalScoreFirst9 / dartsThrown;
            }

            return (decimal)totalScoreFirst9 / 9;
        }

        public async Task<int> CalculateDartsThrown(Guid legId, Guid playerId)
        {
            return await _statsRepository.GetDartsThrownCount(legId, playerId);
        }
        public async Task<decimal> CalculateCheckoutPercentage(Guid legId, Guid playerId)
        {
            var checkoutSuccesses = await _statsRepository.GetCheckoutSuccessCount(legId, playerId);
            var checkoutAttempts = await _statsRepository.GetCheckoutAttemptCount(legId, playerId);
            return checkoutAttempts > 0 ? (checkoutSuccesses / (decimal)checkoutAttempts) * 100 : 0;
        }

        public async Task<int> CalculateCheckoutPoints(Guid legId, Guid playerId)
        {
            return await _statsRepository.GetCheckoutPoints(legId, playerId);
        }

        public async Task<int> CalculateSixtyPlusCount(Guid legId, Guid playerId)
        {
            return await _statsRepository.GetSixtyPlusCount(legId, playerId);
        }

        public async Task<int> CalculateHundredPlusCount(Guid legId, Guid playerId)
        {
            return await _statsRepository.GetHundredPlusCount(legId, playerId);
        }

        public async Task<int> CalculateHundredFortyPlusCount(Guid legId, Guid playerId)
        {
            return await _statsRepository.GetHundredFortyPlusCount(legId, playerId);
        }

        public async Task<int> CalculateOneEightyCount(Guid legId, Guid playerId)
        {
            return await _statsRepository.GetOneEightyCount(legId, playerId);
        }
    }
}
