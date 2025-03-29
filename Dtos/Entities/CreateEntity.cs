using System.ComponentModel.DataAnnotations;
using NepHubAPI.Models;

namespace NepHubAPI.Dtos.Entities;

public record class CreateEntityDTO
{
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    [Required]
    [Url]
    public required string Image { get; set; }
    [Required]public required string Description { get; set; }
    [Required]public required string Position { get; set; }
    public required List<AttributeEntryDTO> Attributes { get; set; } = new();
}

public class AttributeEntryDTO
{
    [Required]
    public required string Key { get; set; }

    [Required]
    public required string Value { get; set; }
}