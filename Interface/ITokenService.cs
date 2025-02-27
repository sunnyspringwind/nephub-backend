using System;
using NepHubAPI.Models;

namespace NepHubAPI.Interface;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
