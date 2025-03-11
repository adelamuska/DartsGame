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

            var turnThrows = await _turnThrowService.GetAll();
            return Ok(turnThrows);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TurnThrowDTO>> GetTurnThrowById(Guid id)
        {

            var turnThrow = await _turnThrowService.GetById(id);
            if (turnThrow == null)
            {
                return NotFound($"Turn throw with ID {id} not found.");
            }
            return Ok(turnThrow);
        }

        [HttpPost]
        public async Task<ActionResult<TurnThrowDTO>> CreateTurnThrow([FromBody] TurnThrowDTO? turnThrowDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTurnThrow = await _turnThrowService.AddTurnThrow(turnThrowDTO);
            return CreatedAtAction(nameof(GetTurnThrowById), new { id = createdTurnThrow.TurnThrowId }, createdTurnThrow);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TurnThrowDTO>> UpdateTurnThrow(Guid id, [FromBody] TurnThrowDTO? turnThrowDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedTurnThrow = await _turnThrowService.UpdateTurnThrow(id, turnThrowDTO);
            return Ok(updatedTurnThrow);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTurnThrow(Guid id)
        {

            await _turnThrowService.DeleteTurnThrow(id);
            return NoContent();

        }
    }
}
