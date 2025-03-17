using DartsGame.Entities;
using DartsGame.Repositories.Statistics;
using DartsGame.Services.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers.Statistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchStatsController : ControllerBase
    {
        private readonly MatchStatsService _matchStats;
        private readonly GameHistoryRepository _gameHistoryRepository;

        public MatchStatsController(MatchStatsService matchStatsService, GameHistoryRepository gameHistoryRepository) 
        { 
            _matchStats = matchStatsService;
            _gameHistoryRepository = gameHistoryRepository;
        }

        [HttpGet("history/{matchId}")]
        public async Task<IActionResult> GetMatchStats(Guid matchId)
        {
            var matchStats = await _gameHistoryRepository.GetMatchStatsByMatchId(matchId);

            if (matchStats == null || !matchStats.Any())
            {
                return NotFound("No stats found for the given match.");
            }

            return Ok(matchStats);
        }

        [HttpGet("match/{matchId}/player/{playerId}")]
        public async Task<ActionResult> GetMatchStatistics(Guid matchId, Guid playerId)
        {
            var statistics = new {

                PPD = await _matchStats.CalculatePPD(matchId, playerId),
                First9PPD = await _matchStats.CalculateFirst9PPD(matchId, playerId),
                LegsWin = await _matchStats.CalculateLegsWon(matchId, playerId),
                SetsWin = await _matchStats.CalculateSetsWon(matchId, playerId),
                CheckoutPercentage = await _matchStats.CalculateCheckoutPercentage(matchId, playerId),
                BestCheckoutPoints = await _matchStats.CalculateBestCheckoutPoints(matchId, playerId),
                SixtyPlusCount = await _matchStats.CalculateSixtyPlusCount(matchId, playerId),
                HundredPlusCount = await _matchStats.CalculateHundredPlusCount(matchId, playerId),
                HundredFortyPlusCount = await _matchStats.CalculateHundredFortyPlusCount(matchId, playerId),
                OneEightyCount = await _matchStats.CalculateOneEightyCount(matchId, playerId)
            };
            return Ok(statistics);
        }
    }
}
