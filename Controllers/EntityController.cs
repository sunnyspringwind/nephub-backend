using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NepHubAPI.Data;
using NepHubAPI.Dtos.Entities;
using NepHubAPI.Models;

namespace NepHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly NepHubContext _context;

        public EntityController(NepHubContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entity = await _context.Entities.ToListAsync();
            return Ok(entity);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await _context.Entities.FindAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> AddPrimeMinister(CreateEntity entityDto)
        {
            var newEntity = new Entity
            {
                Name = entityDto.Name,
                Image = entityDto.Image,
                Attributes = entityDto.Attributes,
                UserId = ClaimsPrincipal.Current.FindFirstValue("Name")
            };
            await _context.Entities.AddAsync(newEntity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newEntity.Id }, newEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntity([FromRoute] int id, CreateEntity entityDto)
        {
            var dbData = await _context.Entities.FindAsync(id);

            if (dbData is null)
            {
                return NotFound();
            }
            dbData.Name = entityDto.Name;
            dbData.Image = entityDto.Image;
            dbData.Attributes = entityDto.Attributes;
            
            await _context.Entities.AddAsync(dbData);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntity([FromRoute] int id)
        {
            var entity = await _context.Entities.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.Entities.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    };
}
