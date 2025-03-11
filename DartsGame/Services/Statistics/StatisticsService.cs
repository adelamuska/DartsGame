using DartsGame.Data;
using DartsGame.Entities;

namespace DartsGame.Services.Statistics
{
    public class StatisticsService
    {
        private readonly AppDbContext _context;
        private readonly LegStatsService _legStatsService;
        private readonly SetStatsService _setStatsService;
        private readonly MatchStatsService _matchStatsService;

        public StatisticsService(AppDbContext context, LegStatsService legStatsService, SetStatsService setStatsService, MatchStatsService matchStatsService)
        {
            _context = context;
            _legStatsService = legStatsService;
            _setStatsService = setStatsService;
            _matchStatsService = matchStatsService;
        }


        public  async Task SaveLegStatistics(Guid legId, Guid playerId)
        {
            var ppd = await _legStatsService.CalculatePPD(legId, playerId);
            var first9PPD = await _legStatsService.CalculateFirst9PPD(legId, playerId);
            var dartsThrown = await _legStatsService.CalculateDartsThrown(legId, playerId);
            var checkoutPercentage = await _legStatsService.CalculateCheckoutPercentage(legId, playerId);
            var checkoutPoints = await _legStatsService.CalculateCheckoutPoints(legId, playerId);
            var sixtyPlus = await _legStatsService.CalculateSixtyPlusCount(legId, playerId);
            var hundredPlus = await _legStatsService.CalculateHundredPlusCount(legId, playerId);
            var hundredFortyPlus = await _legStatsService.CalculateHundredFortyPlusCount(legId, playerId);
            var oneEighty = await _legStatsService.CalculateOneEightyCount(legId, playerId);

            var legStats = new LegStats
            {
                LegId = legId,
                PlayerId = playerId,
                PPD = ppd,
                First9PPD = first9PPD,
                DartsThrown = dartsThrown,
                CheckoutPercentage = checkoutPercentage,
                CheckoutPoints = checkoutPoints,
                Count60Plus = sixtyPlus,
                Count100Plus = hundredPlus,
                Count140Plus = hundredFortyPlus,
                Count180s = oneEighty
            };

            _context.LegStats.Add(legStats);
            await _context.SaveChangesAsync();
        }

        public async Task SaveSetStatistics(Guid setId, Guid playerId)
        {
            var ppd = await _setStatsService.CalculatePPD(setId, playerId);
            var first9PPD = await _setStatsService.CalculateFirst9PPD(setId, playerId);
            var legsWin = await _setStatsService.CalculateLegsWon(setId, playerId);
            var checkoutPercentage = await _setStatsService.CalculateCheckoutPercentage(setId, playerId);
            var checkoutPoints = await _setStatsService.CalculateBestCheckoutPoints(setId, playerId);
            var sixtyPlus = await _setStatsService.CalculateSixtyPlusCount(setId, playerId);
            var hundredPlus = await _setStatsService.CalculateHundredPlusCount(setId, playerId);
            var hundredFortyPlus = await _setStatsService.CalculateHundredFortyPlusCount(setId, playerId);
            var oneEighty = await _setStatsService.CalculateOneEightyCount(setId, playerId);

            var setStats = new SetStats
            {
                SetId = setId,
                PlayerId = playerId,
                PPD = ppd,
                First9PPD = first9PPD,
                LegsWin = legsWin,
                CheckoutPercentage = checkoutPercentage,
                BestCheckoutPoints = checkoutPoints,
                Count60Plus = sixtyPlus,
                Count100Plus = hundredPlus,
                Count140Plus = hundredFortyPlus,
                Count180s = oneEighty
            };

            _context.SetStats.Add(setStats);
            await _context.SaveChangesAsync();
        }

        public async Task SaveMatchStatistics(Guid matchId, Guid playerId)
        {
            var ppd = await _matchStatsService.CalculatePPD(matchId, playerId);
            var first9PPD = await _matchStatsService.CalculateFirst9PPD(matchId, playerId);
            var legsWin = await _matchStatsService.CalculateLegsWon(matchId, playerId);
            var setsWin = await _matchStatsService.CalculateSetsWon(matchId, playerId);
            var checkoutPercentage = await _matchStatsService.CalculateCheckoutPercentage(matchId, playerId);
            var checkoutPoints = await _matchStatsService.CalculateBestCheckoutPoints(matchId, playerId);
            var sixtyPlus = await _matchStatsService.CalculateSixtyPlusCount(matchId, playerId);
            var hundredPlus = await _matchStatsService.CalculateHundredPlusCount(matchId, playerId);
            var hundredFortyPlus = await _matchStatsService.CalculateHundredFortyPlusCount(matchId, playerId);
            var oneEighty = await _matchStatsService.CalculateOneEightyCount(matchId, playerId);

            var matchStats = new MatchStats
            {
                MatchId = matchId,
                PlayerId = playerId,
                PPD = ppd,
                First9PPD = first9PPD,
                LegsWin = legsWin,
                SetsWin = setsWin,
                CheckoutPercentage = checkoutPercentage,
                BestCheckoutPoints = checkoutPoints,
                Count60Plus = sixtyPlus,
                Count100Plus = hundredPlus,
                Count140Plus = hundredFortyPlus,
                Count180s = oneEighty
            };

            _context.MatchStats.Add(matchStats);
            await _context.SaveChangesAsync();
        }
    }
}

