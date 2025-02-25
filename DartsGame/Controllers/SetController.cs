using DartsGame.DTO;
using DartsGame.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DartsGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly SetService _setService;

        public SetController(SetService setService)
        {
            _setService = setService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SetDTO>>> GetAllSets()
        {
            try
            {
                var sets = await _setService.GetAll();
                return Ok(sets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SetDTO>> GetSetById(Guid id)
        {
            try
            {
                var set = await _setService.GetById(id);
                if (set == null)
                {
                    return NotFound($"Set with ID {id} not found.");
                }
                return Ok(set);
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
        public async Task<ActionResult<SetDTO>> CreateSet([FromBody] SetDTO setDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdSet = await _setService.AddSet(setDTO);
                return CreatedAtAction(nameof(GetSetById), new { id = createdSet.SetId }, createdSet);
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
        public async Task<ActionResult<SetDTO>> UpdateSet(Guid id, [FromBody] SetDTO setDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedSet = await _setService.UpdateSet(id, setDTO);
                return Ok(updatedSet);
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
        public async Task<ActionResult> DeleteSet(Guid id)
        {
            try
            {
                await _setService.DeleteSet(id);
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
