namespace NepHubAPI.Dtos.Users;

public record class LoginUserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }

}
