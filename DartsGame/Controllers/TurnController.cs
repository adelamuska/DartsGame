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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TurnDTO>>> GetAllTurns()
        //{

        //    var turns = await _turnService.GetAll();
        //    return Ok(turns);

        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<TurnDTO>> GetTurnById(Guid id)
        {

            var turn = await _turnService.GetById(id);
            if (turn == null)
            {
                return NotFound($"Turn with ID {id} not found.");
            }
            return Ok(turn);
        }

        //[HttpPost]
        //public async Task<ActionResult<TurnDTO>> CreateTurn([FromBody] TurnDTO? turnDTO)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var createdTurn = await _turnService.AddTurn(turnDTO);
        //    return CreatedAtAction(nameof(GetTurnById), new { id = createdTurn.TurnId }, createdTurn);

        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<TurnDTO>> UpdateTurn(Guid id, [FromBody] TurnDTO? turnDTO)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var updatedTurn = await _turnService.UpdateTurn(id, turnDTO);
        //    return Ok(updatedTurn);

        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTurn(Guid id)
        {

            await _turnService.DeleteTurn(id);
            return NoContent();

        }

    }
}

