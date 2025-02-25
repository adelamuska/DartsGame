using AutoMapper;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Mapper;
using DartsGame.Repositories;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegController : ControllerBase
    {
        private readonly LegService _legService;
        private readonly MatchRepository _matchRepository;
        public readonly IMapper _mapper;

        public LegController(LegService legService, MatchRepository matchRepository, IMapper mapper)
        {
            _legService = legService;
            _matchRepository = matchRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LegDTO>>> GetAllLegs()
        {
            try
            {
                var legs = await _legService.GetAll();
                return Ok(legs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LegDTO>> GetLegById(Guid id)
        {
            try
            {
                var leg = await _legService.GetById(id);
                if (leg == null)
                {
                    return NotFound($"Leg with ID {id} not found.");
                }
                return Ok(leg);
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
        public async Task<ActionResult<LegDTO>> CreateLeg([FromBody] LegDTO legDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdLeg = await _legService.AddLeg(legDTO);
                return CreatedAtAction(nameof(GetLegById), new { id = createdLeg.LegId }, createdLeg);
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
        public async Task<ActionResult<LegDTO>> UpdateLeg(Guid id, [FromBody] LegDTO legDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updtatedLeg = await _legService.UpdateLeg(id, legDTO);
                return Ok(updtatedLeg);
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
        public async Task<ActionResult> DeleteLeg(Guid id)
        {
            try
            {
                await _legService.DeleteLeg(id);
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

        [HttpPost("startLeg/{matchId}")]
        public async Task<IActionResult> StartLeg(Guid matchId)
        {
            if (matchId == Guid.Empty)
            {
                return BadRequest("Invalid match ID.");
            }

            try
            {
               
                var match = await _matchRepository.GetById(matchId);

                
                if (match == null)
                {
                    return NotFound("Match not found.");
                }


                var leg = await _legService.StartLeg(match);


                return CreatedAtAction(nameof(GetLegById), new { id = leg.LegId }, leg);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
