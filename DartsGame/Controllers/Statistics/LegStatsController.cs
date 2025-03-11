using DartsGame.Services;
using DartsGame.Services.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers.Statistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegStatsController : ControllerBase
    {
        private readonly LegStatsService _legStats;
        private readonly GameFlowService _gameService;

        public LegStatsController(LegStatsService legStats, GameFlowService gameService)
        {
            _legStats = legStats;
            _gameService = gameService;
        }

        [HttpGet("leg/{legId}/player/{playerId}")]
        public async Task<ActionResult> GetLegStatistics(Guid legId, Guid playerId)
        {

            var statistics =  new
                
                //_gameService.SaveLegStatistics(legId, playerId);
            {
                PPD = await _legStats.CalculatePPD(legId, playerId),
                First9PPD = await _legStats.CalculateFirst9PPD(legId, playerId),
                DartsThrown = await _legStats.CalculateDartsThrown(legId, playerId),
                CheckoutPercentage = await _legStats.CalculateCheckoutPercentage(legId, playerId),
                CheckoutPoints = await _legStats.CalculateCheckoutPoints(legId, playerId),
                SixtyPlusCount = await _legStats.CalculateSixtyPlusCount(legId, playerId),
                HundredPlusCount = await _legStats.CalculateHundredPlusCount(legId, playerId),
                HundredFortyPlusCount = await _legStats.CalculateHundredFortyPlusCount(legId, playerId),
                OneEightyCount = await _legStats.CalculateOneEightyCount(legId, playerId)
            };
            return Ok(statistics);
        }
    }
}
