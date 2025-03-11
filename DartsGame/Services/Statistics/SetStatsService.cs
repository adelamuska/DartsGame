using DartsGame.Data;
using DartsGame.Interfaces;
using DartsGame.Repositories;
using DartsGame.Repositories.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services.Statistics
{
    public class SetStatsService : IStatsService
    {
        private readonly SetStatsRepository _setStatsRepository;
        private readonly LegStatsService _legStatsService;
        private readonly LegStatsRepository _legStatsRepository;

        public SetStatsService(SetStatsRepository setStatsRepository, LegStatsService legStatsService, LegStatsRepository legStatsRepository)
        {
            _setStatsRepository = setStatsRepository;
            _legStatsService = legStatsService;
            _legStatsRepository = legStatsRepository;
        }

        public async Task<decimal> CalculatePPD(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

            double totalScore = 0;
            int totalDartsThrown = 0;

            foreach (var leg in legs)
            {
                decimal ppd = await _legStatsService.CalculatePPD(leg.LegId, playerId);
                int dartsThrown = await _legStatsService.CalculateDartsThrown(leg.LegId, playerId);

                totalScore += (double)ppd * dartsThrown;
                totalDartsThrown += dartsThrown;
            }

            return totalDartsThrown > 0 ? (decimal)(totalScore / totalDartsThrown) : 0;
        }

        public async Task<decimal> CalculateFirst9PPD(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

            double totalFirst9PPD = 0;
            int legCount = 0;

            foreach (var leg in legs)
            {
                var legFirst9PPD = await _legStatsService.CalculateFirst9PPD(leg.LegId, playerId);
                if (legFirst9PPD > 0)
                {
                    totalFirst9PPD += (double)legFirst9PPD;
                    legCount++;
                }
            }

            return legCount > 0 ? (decimal)(totalFirst9PPD / legCount) : 0;
        }


        public async Task<decimal> CalculateCheckoutPercentage(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

           

            decimal totalCheckoutPercentage = 0;
            int legCount = 0;

            foreach (var leg in legs)
            {

                var legCheckoutPercentage = await _legStatsService.CalculateCheckoutPercentage(leg.LegId, playerId);
                totalCheckoutPercentage += legCheckoutPercentage;
                legCount ++;
              
            }
            return legCount > 0 ? totalCheckoutPercentage / legCount : 0;

        }

        public async Task<int> CalculateBestCheckoutPoints(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

            int bestCheckout = 0;

            foreach (var leg in legs)
            {
                int legCheckout = await _legStatsService.CalculateCheckoutPoints(leg.LegId, playerId);
                if (legCheckout > bestCheckout)
                {
                    bestCheckout = legCheckout;
                }
            }

            return bestCheckout;
        }

        public async Task<int> CalculateLegsWon(Guid setId, Guid playerId)
        {
            return await _setStatsRepository.GetLegsWon(setId, playerId);
        }

        public async Task<int> CalculateSixtyPlusCount(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

            int totalCount = 0;

            foreach (var leg in legs)
            {
                totalCount += await _legStatsService.CalculateSixtyPlusCount(leg.LegId, playerId);
            }

            return totalCount;
        }

        public async Task<int> CalculateHundredPlusCount(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

            int totalCount = 0;

            foreach (var leg in legs)
            {
                totalCount += await _legStatsService.CalculateHundredPlusCount(leg.LegId, playerId);
            }

            return totalCount;
        }

        public async Task<int> CalculateHundredFortyPlusCount(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

            int totalCount = 0;

            foreach (var leg in legs)
            {
                totalCount += await _legStatsService.CalculateHundredFortyPlusCount(leg.LegId, playerId);
            }

            return totalCount;
        }

        public async Task<int> CalculateOneEightyCount(Guid setId, Guid playerId)
        {
            var legs = await _setStatsRepository.GetLegsBySetId(setId);
            if (legs == null || !legs.Any()) return 0;

            int totalCount = 0;

            foreach (var leg in legs)
            {
                totalCount += await _legStatsService.CalculateOneEightyCount(leg.LegId, playerId);
            }

            return totalCount;
        }
    }
}