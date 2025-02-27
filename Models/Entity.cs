using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NepHubAPI.Models;

[Table("Entity")]
public class Entity
{
    [Required][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
    [Required][StringLength(50)]public required string Name { get; set; }
    [Required][Url] public required string Image { get; set; }
    [Required] public required string UserId { get; set; }

//navigation property used to store multvalued properties
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public ICollection<AttributeEntry> Attributes { get; set; } = new List<AttributeEntry>();

    
    public AppUser? User { get; set; }


}
