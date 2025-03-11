using DartsGame.Data;
using DartsGame.Entities;
using DartsGame.Interfaces;
using DartsGame.Repositories;
using DartsGame.Repositories.Statistics;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services.Statistics
{
    public class MatchStatsService : IStatsService
    {
        private readonly MatchStatsRepository _matchStatsRepository;
        private readonly SetStatsService _setStatsService;
        private readonly LegStatsService _legStatsService;


        public MatchStatsService(MatchStatsRepository matchStatsRepository,SetStatsService setStatsService, LegStatsService legStatsService)
        {
            _matchStatsRepository = matchStatsRepository;
            _setStatsService = setStatsService;
            _legStatsService = legStatsService;
        }

        public async Task<int> CalculateSetsWon(Guid matchId, Guid playerId)
        {
            return await _matchStatsRepository.GetSetsWon(matchId, playerId);
        }

        public async Task<int> CalculateLegsWon(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            int totalLegsWon = 0;

            foreach (var set in sets)
            {
                totalLegsWon += await _setStatsService.CalculateLegsWon(set.SetId, playerId);
            }

            return totalLegsWon;
        }

        public async Task<decimal> CalculatePPD(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            decimal totalWeightedPPD = 0;
            int totalDartsThrown = 0;

            foreach (var set in sets)
            {
                var setDartsThrown = await _matchStatsRepository.GetTotalDartsThrown(set.SetId, playerId);

                var setPPD = await _setStatsService.CalculatePPD(set.SetId, playerId);

                totalWeightedPPD += setPPD * setDartsThrown;
                totalDartsThrown += setDartsThrown;
            }

            return totalDartsThrown > 0 ? totalWeightedPPD / totalDartsThrown : 0;
        }

        public async Task<decimal> CalculateCheckoutPercentage(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            decimal totalCheckoutPercentage = 0;
            int legCount = 0;

            foreach (var set in sets)
            {
                foreach (var leg in set.Legs)
                {
                    
                    var legCheckoutPercentage = await _legStatsService.CalculateCheckoutPercentage(leg.LegId, playerId);
                    totalCheckoutPercentage += legCheckoutPercentage;
                    legCount++;
                }
            }

            return legCount > 0 ? totalCheckoutPercentage / legCount : 0;
        }

        public async Task<int> CalculateBestCheckoutPoints(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            int bestCheckout = 0;

            foreach (var set in sets)
            {
                int setCheckout = await _setStatsService.CalculateBestCheckoutPoints(set.SetId, playerId);
                if (setCheckout > bestCheckout)
                {
                    bestCheckout = setCheckout;
                }
            }

            return bestCheckout;
        }

        public async Task<int> CalculateSixtyPlusCount(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            int totalCount = 0;

            foreach (var set in sets)
            {
                totalCount += await _setStatsService.CalculateSixtyPlusCount(set.SetId, playerId);
            }

            return totalCount;
        }

        public async Task<int> CalculateHundredPlusCount(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            int totalCount = 0;

            foreach (var set in sets)
            {
                totalCount += await _setStatsService.CalculateHundredPlusCount(set.SetId, playerId);
            }

            return totalCount;
        }

        public async Task<int> CalculateHundredFortyPlusCount(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            int totalCount = 0;

            foreach (var set in sets)
            {
                totalCount += await _setStatsService.CalculateHundredFortyPlusCount(set.SetId, playerId);
            }

            return totalCount;
        }

        public async Task<int> CalculateOneEightyCount(Guid matchId, Guid playerId)
        {
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            int totalCount = 0;

            foreach (var set in sets)
            {
                totalCount += await _setStatsService.CalculateOneEightyCount(set.SetId, playerId);
            }

            return totalCount;
        }

        public async Task<decimal> CalculateFirst9PPD(Guid matchId, Guid playerId)
        {
            //metoda me leg
        //    var legs = await _matchStatsRepository.GetLegsByMatchId(matchId);
        //    if (legs == null || !legs.Any()) return 0;

            //    double totalFirst9PPD = 0;
            //    int legCount = 0;

            //    foreach (var leg in legs)
            //    {
            //        var legFirst9PPD = await _legStatsService.CalculateFirst9PPD(leg.LegId, playerId);
            //        if (legFirst9PPD > 0)
            //        {
            //            totalFirst9PPD += (double)legFirst9PPD;
            //            legCount++;
            //        }
            //    }

            //    return legCount > 0 ? (decimal)(totalFirst9PPD / legCount) : 0;
            //}

            // Metoda me set
            var sets = await _matchStatsRepository.GetSetsByMatchId(matchId);
            if (sets == null || !sets.Any()) return 0;

            double totalFirst9PPD = 0;
            int setCount = 0;

            foreach (var set in sets)
            {
                var setFirst9PPD = await _setStatsService.CalculateFirst9PPD(set.SetId, playerId);
                if (setFirst9PPD > 0)
                {
                    totalFirst9PPD += (double)setFirst9PPD;
                    setCount++;
                }
            }

            return setCount > 0 ? (decimal)(totalFirst9PPD / setCount) : 0;
        }


    }
}