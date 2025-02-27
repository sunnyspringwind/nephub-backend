using System.ComponentModel.DataAnnotations;

namespace NepHubAPI.Dtos.Users;

public record class ViewUserDto
{
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Bio  { get; set; }

        public string? ImageUrl {get; set;}
        public  string? Token { get; set; } //using newtonsoft text serliazion service
        public  string? UserId { get; set; }
}
