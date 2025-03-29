using System.ComponentModel.DataAnnotations;
using NepHubAPI.Models;

namespace NepHubAPI.Dtos.Entities;

public class ViewEntityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
      public required string Description { get; set; }
   public required string Position { get; set; }
    
    public List<AttributeEntryDTO> Attributes { get; set; } = new();
}
