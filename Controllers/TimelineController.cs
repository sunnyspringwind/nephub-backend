using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NepHubAPI.Data;
using NepHubAPI.Dtos.Timeline;
using NepHubAPI.Models;

namespace NepHubAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimelineController(NepHubContext context) : ControllerBase
    {
           private readonly NepHubContext _context = context;

         [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var list =  await _context.Timeline.ToListAsync();
            return Ok(list);
        }

        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var scene = await _context.Timeline.FindAsync(id);

            if (scene == null) {
                return NotFound();
            }
            return Ok(scene);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] AddEventDto eventDto)
        {
            if (eventDto == null)
            {
                return BadRequest();
            }

            var scene = new Timeline
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                Date = eventDto.Date,
                Image = eventDto.Image
            };

            await _context.Timeline.AddAsync(scene);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = scene.Id }, scene);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent([FromRoute] int id, [FromBody] UpdateEventDto updatedDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (updatedDto == null) {
                return BadRequest();
            }

            var dbData = await _context.Timeline.FindAsync(id);

            if (dbData == null) 
            {
                return BadRequest();
            }

                dbData.Title = updatedDto.Title;
                dbData.Description = updatedDto.Description;
                dbData.Date = updatedDto.Date;          
                dbData.Image = updatedDto.Image;

            _context.Timeline.Update(dbData);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id) {
            var scene = await _context.Timeline.FindAsync(id);
            if (scene == null) {
                return NotFound();
            }
            
            _context.Timeline.Remove(scene);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
