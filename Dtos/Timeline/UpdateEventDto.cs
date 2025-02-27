using System.ComponentModel.DataAnnotations;

namespace NepHubAPI.Dtos.Timeline;

public record class UpdateEventDto
{
    [Required][StringLength(100)] public required string Title { get; set; }
    [Required][StringLength(500)] public required string Description {get; set;}
    [Required] public required DateOnly Date { get; set; }

    [Url] public string? Image { get; set; }
}
