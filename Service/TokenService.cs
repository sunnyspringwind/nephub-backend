using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NepHubAPI.Interface;
using NepHubAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService

{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
    }

    public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
{
    var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.NameId, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
    };

    // Get user roles and add them as claims
    var roles = await userManager.GetRolesAsync(user);
    foreach (var role in roles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
    var now = DateTime.UtcNow; // Ensure consistency
    var expires = now.AddMinutes(60);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        // Expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["JWT:TokenValidityInMinutes"])),
        NotBefore = now,
        Expires = expires,
        IssuedAt = now,
        SigningCredentials = creds,
        Issuer = _config["JWT:Issuer"],
        Audience = _config["JWT:Audience"]
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
}
}
