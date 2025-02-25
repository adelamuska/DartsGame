using DartsGame.DTO;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetAllPlayers()
        {
            try
            {
                var players = await _playerService.GetAll();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayerById(Guid id)
        {
            try
            {
                var player = await _playerService.GetById(id);
                if (player == null)
                {
                    return NotFound($"Player with ID {id} not found.");
                }
                return Ok(player);
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
        public async Task<ActionResult<PlayerDTO>> CreatePlayer([FromBody] PlayerDTO playerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdPlayer = await _playerService.AddPlayer(playerDTO);
                return CreatedAtAction(nameof(GetPlayerById), new { id = createdPlayer.PlayerId }, createdPlayer);
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
        public async Task<ActionResult<PlayerDTO>> UpdatePlayer(Guid id, [FromBody] PlayerDTO playerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedPlayer = await _playerService.UpdatePlayer(id, playerDTO);
                return Ok(updatedPlayer);
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
        public async Task<ActionResult> DeletePlayer(Guid id)
        {
            try
            {
                await _playerService.DeletePlayer(id);
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

    }
}

