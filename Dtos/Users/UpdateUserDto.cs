using System.ComponentModel.DataAnnotations;

namespace NepHubAPI.Dtos.Users;

public record class UpdateUserDto
{
    public string? ImageUrl { get; set; }
    [StringLength(20)] public required string Username { get; set; }
    public string? Email { get; set; }
    public string? Bio { get; set; }
}
