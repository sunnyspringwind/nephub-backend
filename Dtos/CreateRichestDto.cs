using System.ComponentModel.DataAnnotations;

namespace NepHubAPI.Dtos
{
    public record CreateRichestDto
    {
    [Required][StringLength(50)] public required string Name { get; set; }

    [Required][Url] public required string Image { get; set; }   

    [Required] [StringLength(50)] public required string Designation {get; set; }

    [Required] public required string NetWorth { get; set; }
    }
}