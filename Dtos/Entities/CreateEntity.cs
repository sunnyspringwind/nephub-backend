using System.ComponentModel.DataAnnotations;
using NepHubAPI.Models;

namespace NepHubAPI.Dtos.Entities;

public record class CreateEntity
{

    [Required][StringLength(50)]public required string Name { get; set; }
    [Required][Url] public required string Image { get; set; }
    [Required] public required ICollection<AttributeEntry> Attributes { get; set; }



}
