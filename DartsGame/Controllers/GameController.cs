using DartsGame.Data;
using DartsGame.RequestDTOs;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {

        private readonly GameService _gameService;
        private readonly AppDbContext _context;

        public GameController(GameService gameService, AppDbContext context)
        {
            _gameService = gameService;
            _context = context;
        }


        [HttpPost("validate-leg-completion")]
        public async Task<IActionResult> ValidateLegCompletion([FromBody] ValidateLegCompletionRequest request)
        {

            var currentLeg = await _context.Legs.FindAsync(request.LegId);
            if (currentLeg == null) { 
                return NotFound("Leg not found");
            }

            await _gameService.ValidateLegCompletion(currentLeg, request.TurnScore, request.LastThrow);
            return Ok();
        }

        [HttpPost("create-new-leg")]
        public async Task<IActionResult> CreateNewLegIfNeeded([FromBody] Guid setId)
        {
            await _gameService.CreateNewLegIfNeeded(setId);
            return Ok();

        }

        [HttpPost("check-set-completion")]
        public async Task<IActionResult> CheckSetCompletion([FromBody] Guid setId)
        {
            await _gameService.CreateNewLegIfNeeded(setId);
            return Ok();
        }

        [HttpPost("check-match-completion")]
        public async Task<IActionResult> CheckMatchCompletion([FromBody] Guid matchId)
        {
            var match = await _context.Matches.FindAsync(matchId);
            if (match == null)
            {
                return NotFound("Match not found");
            }

            return Ok();
        }

        [HttpPost("create-new-set")]
        public async Task<IActionResult> CreateNewSetIfNeeded([FromBody] Guid matchId)
        {

            var match = await _context.Matches.FindAsync(matchId);
            if (match == null)
            {
                return NotFound("Match not found.");
            }
            return Ok();
        }
    }
}
