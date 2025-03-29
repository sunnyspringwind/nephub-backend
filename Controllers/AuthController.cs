using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var userDtos = new List<ViewUserDto>();

            foreach (var user in users)
            {
                var token = await _tokenService.CreateToken(user, _userManager);
                userDtos.Add(new ViewUserDto
                {
                    Username = user.UserName!,
                    Email = user.Email!,
                    Bio = user.Bio,
                    ImageUrl = user.ImageUrl,
                    Token = token,
                    UserId = user.Id
                });
            }

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
                if (existingUser?.Email == newUserDto.Email?.ToLower())
                    return BadRequest(new {message = "Email already exists"});
                if (existingUser?.UserName == newUserDto.Username)
                    return BadRequest(new {message = "Username already exists"});
              
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
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginDto)
        {   
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Find user by username or email
                var user = await _userManager.FindByNameAsync(loginDto.Email) ??
                           await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null)
                    return Unauthorized("Invalid username or password");

                // Check password
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

                if (!isPasswordValid)
                    return Unauthorized("Invalid username or password");

                // Get user roles (for display purposes, but the real roles are in the token)
                var roles = await _userManager.GetRolesAsync(user);

                // Generate JWT token with role claims
                var token = await _tokenService.CreateToken(user, _userManager);

                // Return user info
                return Ok(new
                {
                    id = user.Id,
                    username = user.UserName,
                    email = user.Email,
                    imageUrl = user.ImageUrl,
                    bio = user.Bio,
                    token = token
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Invalid token" });

            var user = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);
            Console.WriteLine($"Updating user: {user.UserName}, Email: {user.Email}");

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = (updateUserDto.Username == "")? user.UserName : updateUserDto.Username;
            user.Email = (updateUserDto.Email== "")? user.Email : updateUserDto.Email;
            user.ImageUrl = (updateUserDto.ImageUrl== "")? user.ImageUrl : updateUserDto.ImageUrl;
            user.Bio = (updateUserDto.Bio == "")? user.Bio : updateUserDto.Bio;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new ViewUserDto
            {
                Username = user.UserName!,
                Email = user.Email!,
                ImageUrl = user.ImageUrl,
                Bio = user.Bio,
                Token = await _tokenService.CreateToken(user, _userManager),
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