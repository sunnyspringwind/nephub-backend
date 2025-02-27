using System.ComponentModel.DataAnnotations;

namespace NepHubAPI.Dtos.Users;

public record class UpdateUserDto
{
    [Required][StringLength(50)]public required string Name { get; set; }
    [Url] public string? Image { get; set; }
    [Required] public required int FavouriteNumber { get; set; }
    [Required][StringLength(20)] public required string Username{ get; set; }
}
