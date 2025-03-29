using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
            var entities = await _context.Entities
                .Include(e => e.Attributes)
                .Select(e => new ViewEntityDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Image = e.Image,
                    UserId = e.UserId,
                    Description = e.Description,
                    Position = e.Position,
                    Attributes = e.Attributes.Select(a => new AttributeEntryDTO
                    {
                        Key = a.Key,
                        Value = a.Value
                    }).ToList()
                })
                .ToListAsync();

            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var item = await _context.Entities.Include(e => e.Attributes).Where(e => e.Id == id).FirstOrDefaultAsync();

            ViewEntityDto entitydata = new ViewEntityDto
            {
                Id = id,
                Name = item.Name,
                Image = item.Image,
                UserId = item.UserId,
                Description = item.Description,
                Position = item.Position,
                Attributes = item.Attributes.Select(a => new AttributeEntryDTO
                {
                    Key = a.Key,
                    Value = a.Value
                }).ToList()
            };
            if (item is null)
            {
                return NotFound();
            }
            return Ok(entitydata);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddEntity(CreateEntityDTO entityDto)
        {
            var newEntity = new Entity
            {
                Name = entityDto.Name,
                Image = entityDto.Image,
                Description = entityDto.Description,
                Position = entityDto.Position,
                Attributes = [.. entityDto.Attributes.Select(a => new AttributeEntry
        {
            Key = a.Key,
            Value = a.Value
        })],
                UserId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty
            };

            await _context.Entities.AddAsync(newEntity);
            await _context.SaveChangesAsync();

            // Convert to DTO to avoid self-referencing loop
            var entityDtoResponse = new ViewEntityDto
            {
                Id = newEntity.Id,
                Name = newEntity.Name,
                Image = newEntity.Image,
                UserId = newEntity.UserId,
                Description = newEntity.Description,
                Position = newEntity.Position,
                Attributes = newEntity.Attributes.Select(a => new AttributeEntryDTO
                {
                    Key = a.Key,
                    Value = a.Value
                }).ToList()
            };

            return CreatedAtAction(nameof(GetById), new { id = newEntity.Id }, entityDtoResponse);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntity(int id, CreateEntityDTO entityDto)
        {
            var entity = await _context.Entities
                .Include(e => e.Attributes)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null) return NotFound();
        
            entity.Name = entityDto.Name;
            entity.Image = entityDto.Image;

            // Convert to dictionary for easy lookup
            var existingAttributes = entity.Attributes.ToDictionary(a => a.Key);

            foreach (var newAttr in entityDto.Attributes)
            {
                if (existingAttributes.TryGetValue(newAttr.Key, out var existingAttr))
                {
                    // Update value if different
                    if (existingAttr.Value != newAttr.Value)
                    {
                        existingAttr.Value = newAttr.Value;
                    }
                    existingAttributes.Remove(newAttr.Key); // Mark as processed
                }
                else
                {
                    // Add new attribute
                    entity.Attributes.Add(new AttributeEntry
                    {
                        Key = newAttr.Key,
                        Value = newAttr.Value
                    });
                }
            }
            // Remove attributes that were not in the new request
            entity.Attributes = entity.Attributes.Where(a => !existingAttributes.ContainsKey(a.Key)).ToList();

            await _context.SaveChangesAsync();
            return Ok( new ViewEntityDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Image = entity.Image,
                UserId = entity.UserId,
                Description = entity.Description,
                Position = entity.Position,
                Attributes = entity.Attributes.Select(a => new AttributeEntryDTO
                {
                    Key = a.Key,
                    Value = a.Value
                }).ToList()
            });
        }

        [Authorize]
        [HttpGet("user-hall-of-fame")]
        public async Task<IActionResult> GetUserHallOfFame()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            var items = await _context.Entities
            .Include(e => e.Attributes)
            .Where(e => e.UserId == userId)
            .ToListAsync();

            if (items.Count == 0) return NotFound();

            var entityData = items.Select(item => new ViewEntityDto
            {
            Id = item.Id,
            Name = item.Name,
            Image = item.Image,
            UserId = item.UserId,
            Description = item.Description,
            Position = item.Position,
            Attributes = item.Attributes.Select(a => new AttributeEntryDTO
            {
                Key = a.Key,
                Value = a.Value
            }).ToList()
            });

            return Ok(entityData);
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
