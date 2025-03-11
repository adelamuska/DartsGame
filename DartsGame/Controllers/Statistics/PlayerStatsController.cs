using DartsGame.DTOs;
using DartsGame.Services.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers.Statistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerStatsController : ControllerBase
    {
        public readonly PlayerStatsService _playerStatsService;

        public PlayerStatsController(PlayerStatsService playerStatsService) {
            _playerStatsService = playerStatsService;
        }

        [HttpGet("{playerId}")]
        public async Task<ActionResult<PlayerStatsDTO>> GetPlayerStats(Guid playerId, [FromQuery] int recentLegs = 10)
        {
            if(playerId == Guid.Empty)
            {
                return BadRequest("Invalid player ID. ");
            }

            var playerStats = await _playerStatsService.GetPlayerStats(playerId);

           return Ok(playerStats);
        }
    }
}
