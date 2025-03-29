using System;
using Microsoft.AspNetCore.Identity;
using NepHubAPI.Models;

namespace NepHubAPI.Interface;

public interface ITokenService
{
  Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager);
}
