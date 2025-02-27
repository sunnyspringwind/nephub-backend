using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace NepHubAPI.Dtos.Users;

public record class CreateUserDto
{
    [EmailAddress] public string? Email { get; set; }
    [Required][StringLength(20)] public required string Username{ get; set; }
    [Required] public required string Password{ get; set; }
}
