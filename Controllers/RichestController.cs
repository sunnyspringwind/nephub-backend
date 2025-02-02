using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NepHubAPI.Data;
using NepHubAPI.Dtos;
using NepHubAPI.Models;

namespace NepHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RichestController : ControllerBase
    {
        private readonly NepHubContext _context;

        public RichestController(NepHubContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Richest>>> GetAll()
        {
            return await _context.Richest.ToListAsync();
        }

        //get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var richest = await _context.Richest.FindAsync(id);

            if (richest == null) {
                return NotFound();
            }
            return Ok(richest);
        }

        [HttpPost]

        public async Task<IActionResult> CreateRichest([FromBody] CreateRichestDto richestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (richestDto == null)
            {
                return NotFound();
            }

            var richest = new Richest
            {
                Name = richestDto.Name,
                Image = richestDto.Image,
                Designation = richestDto.Designation,
                NetWorth = richestDto.NetWorth
            };

            await _context.Richest.AddAsync(richest);
            await _context.SaveChangesAsync();

            var response = new Richest
            {
                Id = richest.Id,
                Name = richestDto.Name,
                Image = richestDto.Image,
                Designation = richestDto.Designation,
                NetWorth = richestDto.NetWorth
            };

            return CreatedAtAction(nameof(GetById), new { id = richest.Id }, response);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> EditProfile([FromRoute] int id, [FromBody] UpdateRichestDto updatedDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (updatedDto == null) {
                return NotFound();
            }

            var dbData = await _context.Richest.FindAsync(id);

            if (dbData == null) 
            {
                return NotFound();
            }

                dbData.Name = updatedDto.Name;
                dbData.Image = updatedDto.Image;
                dbData.Designation = updatedDto.Designation;
                dbData.NetWorth = updatedDto.NetWorth;
          

            _context.Richest.Update(dbData);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteRichest([FromRoute] int id) {
            var richest = await _context.Richest.FindAsync(id);
            if (richest == null) {
                return NotFound();
            }

            _context.Richest.Remove(richest);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
