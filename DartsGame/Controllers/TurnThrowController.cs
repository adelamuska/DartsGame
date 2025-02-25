using DartsGame.DTO;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnThrowController : ControllerBase
    {
        private readonly TurnThrowService _turnThrowService;

        public TurnThrowController(TurnThrowService turnThrowService)
        {
            _turnThrowService = turnThrowService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurnThrowDTO>>> GetAllTurnThrows()
        {
            try
            {
                var turnThrows = await _turnThrowService.GetAll();
                return Ok(turnThrows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurnThrowDTO>> GetTurnThrowById(Guid id)
        {
            try
            {
                var turnThrow = await _turnThrowService.GetById(id);
                if (turnThrow == null)
                {
                    return NotFound($"Turn throw with ID {id} not found.");
                }
                return Ok(turnThrow);
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
        public async Task<ActionResult<TurnThrowDTO>> CreateTurnThrow([FromBody] TurnThrowDTO turnThrowDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdTurnThrow = await _turnThrowService.AddTurnThrow(turnThrowDTO);
                return CreatedAtAction(nameof(GetTurnThrowById), new { id = createdTurnThrow.TurnThrowId }, createdTurnThrow);
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
        public async Task<ActionResult<TurnThrowDTO>> UpdateTurnThrow(Guid id, [FromBody] TurnThrowDTO turnThrowDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTurnThrow = await _turnThrowService.UpdateTurnThrow(id, turnThrowDTO);
                return Ok(updatedTurnThrow);
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
        public async Task<ActionResult> DeleteTurnThrow(Guid id)
        {
            try
            {
                await _turnThrowService.DeleteTurnThrow(id);
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
