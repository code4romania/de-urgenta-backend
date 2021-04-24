using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Models.DTOs.Responses;
using DeUrgenta.User.Api.Services;
using DeUrgenta.User.Api.Services.Emailing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace DeUrgenta.User.Api.Controller
{
    [Route("auth")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IJwtService jwtService, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _emailSender = emailSender;
            _configuration = configuration;
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
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl =
                    $"confirmEmail.html?userId={newUser.Id}&code={code}";

                await SendRegistrationEmail(newUser.UserName, user.Email, callbackUrl);
                return Ok("Email was sent");
            }

            return BadRequest(new RegistrationResponse
            {
                Errors = identityResult.Errors.Select(x => x.Description).ToList(),
                Success = false
            });
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromBody] UserConfirmationDto confirmationRequest)
        {
            var user = await _userManager.FindByEmailAsync(confirmationRequest.Email);
            if (user == null)
            {
                return BadRequest();
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmationRequest.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded) { return Ok(); }
            return BadRequest();
        }

        private async Task SendRegistrationEmail(string userName, string userEmail, string callbackUrl)
        {
            EmailRequestModel email = new EmailRequestModel
            {
                Address = userEmail,
                PlaceholderContent = new Dictionary<string, string>(),
                TemplateType = EmailTemplate.AccountConfirmation,
                SenderName = _configuration.GetValue<string>("AdminFromEmail"),
                Subject = "[Aplicatia de urgenta] Confirmare adresa email"
            };
            email.PlaceholderContent.Add("name", HtmlEncoder.Default.Encode(userName));
            email.PlaceholderContent.Add("confirmationLink", callbackUrl);

            await _emailSender.SendAsync(email);
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