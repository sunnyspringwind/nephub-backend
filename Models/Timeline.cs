using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NepHubAPI.Models;
[Table("Timeline")]
public class Timeline
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }
    [Required][StringLength(100)] public required string Title { get; set; }
    [Required][StringLength(500)] public required string Description {get; set;}
    [Required] public required DateOnly Date { get; set; }

    [Url] public string? Image { get; set; }
}
