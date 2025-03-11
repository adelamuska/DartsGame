using DartsGame.DTO;
using DartsGame.RequestDTOs;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;

        public MatchController(MatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatchDTO>>> GetAllMatches()
        {
            var matches = await _matchService.GetAll();
            return Ok(matches);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDTO>> GetMatchById(Guid id)
        {
            var match = await _matchService.GetById(id);
            if (match == null)
            {
                return NotFound($"Match with ID {id} not found.");
            }
            return Ok(match);

        }

        [HttpPost]
        public async Task<ActionResult<MatchDTO>> CreateMatch([FromBody] MatchDTO? matchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMatch = await _matchService.AddMatch(matchDTO);
            return CreatedAtAction(nameof(GetMatchById), new { id = createdMatch.MatchId }, createdMatch);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MatchDTO>> UpdateMatch(Guid id, [FromBody] MatchDTO? matchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedMatch = await _matchService.UpdateMatch(id, matchDTO);
            return Ok(updatedMatch);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(Guid id)
        {

            await _matchService.DeleteMatch(id);
            return NoContent();

        }

    }
}

