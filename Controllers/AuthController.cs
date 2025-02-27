using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NepHubAPI.Data;
using NepHubAPI.Dtos.Users;
using NepHubAPI.Interface;
using NepHubAPI.Models;

namespace NepHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        // private readonly IEmailSender _emailSender;
        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = users.Select(user => new ViewUserDto
            {
                Username = user.UserName!,
                Email = user.Email!,
                Token = _tokenService.CreateToken(user),
                UserId = user.Id
            }).ToList();
            return Ok(userDtos);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ViewUserDto>> Register([FromBody] CreateUserDto newUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var existingUser = await _userManager.Users.Where(user => user.UserName == newUserDto.Username || user.Email == newUserDto.Email).FirstOrDefaultAsync();
                if (existingUser?.UserName != null)
                    return BadRequest("Username already exists");
                if (existingUser?.Email != null)
                    return BadRequest("Email already exists");
                var newUser = new AppUser
                {
                    UserName = newUserDto.Username,
                    Email = newUserDto.Email,
                };

                var createdUser = await _userManager.CreateAsync(newUser, newUserDto.Password);

                if (createdUser.Succeeded)
                {
                    var addRole = await _userManager.AddToRoleAsync(newUser, "USER");
                    if (addRole.Succeeded)
                    {
                        return Ok(
                            new ViewUserDto
                            {
                                Username = newUser.UserName,
                                Email = newUser.Email!
                            }
                        );
                    }
                    else
                        return StatusCode(500, addRole.Errors);
                }
                else
                    return StatusCode(500, createdUser.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.Username);

            if (user == null)
            {
                return NotFound("Username does not exist");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Invalid username or password");

            return Ok(new ViewUserDto
            {
                Username = user.UserName!,
                Email = user.Email!,
                ImageUrl = user.ImageUrl,
                Bio = user.Bio,
                Token = _tokenService.CreateToken(user),

            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return NoContent();
        }
    }
}