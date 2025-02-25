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
            try
            {
                var matches = await _matchService.GetAll();
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatchDTO>> GetMatchById(Guid id)
        {
            try
            {
                var match = await _matchService.GetById(id);
                if (match == null)
                {
                    return NotFound($"Match with ID {id} not found.");
                }
                return Ok(match);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<MatchDTO>> CreateMatch([FromBody] MatchDTO matchDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdMatch = await _matchService.AddMatch(matchDTO);
                return CreatedAtAction(nameof(GetMatchById), new { id = createdMatch.MatchId }, createdMatch);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<MatchDTO>> UpdateMatch(Guid id, [FromBody] MatchDTO matchDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedMatch = await _matchService.UpdateMatch(id, matchDTO);
                return Ok(updatedMatch);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMatch(Guid id)
        {
            try
            {
                await _matchService.DeleteMatch(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("start")]
        public async Task<ActionResult> StartMatch([FromBody] StartMatchRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid match details.");
            }

            try
            {
                var match = await _matchService.StartMatch(request.StartingScore, request.NumberOfSets, request.NumberOfPlayers, request.PlayerNames);
                return Ok(match);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); 
            }
        }

    }
}

