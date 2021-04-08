using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Models.DTOs.Responses;
using DeUrgenta.User.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.User.Api.Controller
{
    [Route("auth")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                // User already exits, return same result as per valid email to avoid user enumeration.
                return Ok("Email was sent");
            }

            var newUser = new IdentityUser { Email = user.Email, UserName = user.Email };
            var identityResult = await _userManager.CreateAsync(newUser, user.Password);

            if (identityResult.Succeeded)
            {
                return Ok("Email was sent");
            }

            return BadRequest(new RegistrationResponse
            {
                Errors = identityResult.Errors.Select(x => x.Description).ToList(),
                Success = false
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            var badRegistrationResponse = new RegistrationResponse()
            {
                Errors = new List<string> {
                    "Invalid login request"
                },
                Success = false
            };

            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return BadRequest(badRegistrationResponse);
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

            if (!isCorrect)
            {
                return BadRequest(badRegistrationResponse);
            }

            var jwtToken = _jwtService.GenerateJwtToken(existingUser);

            return Ok(jwtToken);
        }
    }
}