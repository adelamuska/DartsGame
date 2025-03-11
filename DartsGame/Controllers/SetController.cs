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
            var sets = await _setService.GetAll();
            return Ok(sets);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SetDTO>> GetSetById(Guid id)
        {
            var set = await _setService.GetById(id);
            if (set == null)
            {
                return NotFound($"Set with ID {id} not found.");
            }
            return Ok(set);

        }

        [HttpPost]
        public async Task<ActionResult<SetDTO>> CreateSet([FromBody] SetDTO? setDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdSet = await _setService.AddSet(setDTO);
            return CreatedAtAction(nameof(GetSetById), new { id = createdSet.SetId }, createdSet);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SetDTO>> UpdateSet(Guid id, [FromBody] SetDTO? setDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedSet = await _setService.UpdateSet(id, setDTO);
            return Ok(updatedSet);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSet(Guid id)
        {

            await _setService.DeleteSet(id);
            return NoContent();

        }
    }
}
