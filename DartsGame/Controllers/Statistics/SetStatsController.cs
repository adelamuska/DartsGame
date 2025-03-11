using DartsGame.Entities;
using DartsGame.Services.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers.Statistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetStatsController : ControllerBase
    {
        private readonly SetStatsService _setStats;

        public SetStatsController(SetStatsService setStats)
        {
            _setStats = setStats;
        }

        [HttpGet("set/{setId}/player/{playerId}")]
        public async Task<ActionResult> GetSetStatistics(Guid setId, Guid playerId)
        {
            var statistics = new
            {
                PPD = await _setStats.CalculatePPD(setId, playerId),
                First9PPD = await _setStats.CalculateFirst9PPD(setId, playerId),
                LegsWin = await _setStats.CalculateLegsWon(setId, playerId),
                CheckoutPercentage = await _setStats.CalculateCheckoutPercentage(setId, playerId),
                BestCheckoutPoints = await _setStats.CalculateBestCheckoutPoints(setId, playerId),
                SixtyPlusCount = await _setStats.CalculateSixtyPlusCount(setId, playerId),
                HundredPlusCount = await _setStats.CalculateHundredPlusCount(setId, playerId),
                HundredFortyPlusCount = await _setStats.CalculateHundredFortyPlusCount(setId, playerId),
                OneEightyCount = await _setStats.CalculateOneEightyCount(setId, playerId)
            };
            return Ok(statistics);
        }
    }
}
