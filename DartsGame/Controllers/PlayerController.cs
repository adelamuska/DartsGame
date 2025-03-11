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
            var players = await _playerService.GetAll();
            return Ok(players);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayerById(Guid id)
        {

            var player = await _playerService.GetById(id);
            if (player == null)
            {
                return NotFound($"Player with ID {id} not found.");
            }
            return Ok(player);

        }

        [HttpPost]
        public async Task<ActionResult<PlayerDTO>> CreatePlayer([FromBody] PlayerDTO? playerDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPlayer = await _playerService.AddPlayer(playerDTO);
            return CreatedAtAction(nameof(GetPlayerById), new { id = createdPlayer.PlayerId }, createdPlayer);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PlayerDTO>> UpdatePlayer(Guid id, [FromBody] PlayerDTO? playerDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedPlayer = await _playerService.UpdatePlayer(id, playerDTO);
            return Ok(updatedPlayer);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlayer(Guid id)
        {

            await _playerService.DeletePlayer(id);
            return NoContent();

        }

    }
}

