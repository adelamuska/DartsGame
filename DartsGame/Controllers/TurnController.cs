using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Repositories;
using DartsGame.RequestDTOs;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnController : ControllerBase
    {
        private readonly TurnService _turnService;
        private readonly MatchRepository _matchRepository;
        private readonly MatchService _matchService;



        public TurnController(TurnService turnService, MatchService matchService, MatchRepository matchRepository)
        {
            _turnService = turnService;
            _matchService = matchService;
            _matchRepository = matchRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurnDTO>>> GetAllTurns()
        {
            try
            {
                var turns = await _turnService.GetAll();
                return Ok(turns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurnDTO>> GetTurnById(Guid id)
        {
            try
            {
                var turn = await _turnService.GetById(id);
                if (turn == null)
                {
                    return NotFound($"Turn with ID {id} not found.");
                }
                return Ok(turn);
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
        public async Task<ActionResult<TurnDTO>> CreateTurn([FromBody] TurnDTO turnDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdTurn = await _turnService.AddTurn(turnDTO);
                return CreatedAtAction(nameof(GetTurnById), new { id = createdTurn.TurnId }, createdTurn);
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
        public async Task<ActionResult<TurnDTO>> UpdateTurn(Guid id, [FromBody] TurnDTO turnDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTurn = await _turnService.UpdateTurn(id, turnDTO);
                return Ok(updatedTurn);
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
        public async Task<ActionResult> DeleteTurn(Guid id)
        {
            try
            {
                await _turnService.DeleteTurn(id);
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


        [HttpPost("changeTurn")]
        public async Task<ActionResult> ChangeTurn([FromBody] ChangeTurnRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var match = await _matchRepository.GetById(request.MatchId);
                if (match == null)
                {
                    return NotFound($"Match with ID {request.MatchId} not found.");
                }


                var turnThrows = new TurnThrow
                {
                    Throw1 = request.Throw1,
                    Throw2 = request.Throw2,
                    Throw3 = request.Throw3
                };

                await _turnService.ChangeTurn(match, turnThrows);

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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

