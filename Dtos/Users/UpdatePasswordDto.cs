using System.ComponentModel.DataAnnotations;

namespace NepHubAPI.Dtos.Users;

public record class UpdatePasswordDto
{
    [Required] public required string Email { get; set; }
    [Required] public required string OldPassword{ get; set; }

    [Required] public required string NewPassword{ get; set; }
}
