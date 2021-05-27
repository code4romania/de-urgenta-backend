using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Models.DTOs.Responses;
using DeUrgenta.User.Api.Notifications;
using DeUrgenta.User.Api.Services;
using DeUrgenta.User.Api.Services.Emailing;
using MediatR;
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
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IJwtService _jwtService;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private string _senderName;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IApplicationUserManager applicationUserManager,
            IJwtService jwtService,
            IMediator mediator,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _applicationUserManager = applicationUserManager;
            _jwtService = jwtService;
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationDto user)
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
                await _applicationUserManager.CreateApplicationUserAsync(user, newUser.Id);
                string callbackUrl = await GetCallbackUrlAsync(newUser);

                await SendRegistrationEmail(newUser.UserName, user.Email, callbackUrl);
                return Ok("Email was sent");
            }

            return BadRequest(new RegistrationResponse
            {
                Errors = identityResult.Errors.Select(x => x.Description).ToList(),
                Success = false
            });
        }

        private async Task<string> GetCallbackUrlAsync(IdentityUser newUser)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var confirmationUrl = _configuration.GetValue<string>("ConfirmationUrl");
            var callbackUrl = $"{confirmationUrl}?userId={newUser.Id}&code={code}";
            return callbackUrl;
        }

        [HttpPost]
        [Route("confirm")]
        public async Task<IActionResult> ConfirmEmail([FromBody] UserConfirmationDto confirmationRequest)
        {
            var user = await _userManager.FindByIdAsync(confirmationRequest.UserId);
            if (user == null)
            {
                return BadRequest();
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmationRequest.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded) { return Ok(); }
            return BadRequest();
        }

        [HttpPost]
        [Route("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmail request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                var hasConfirmedEmail = await _userManager.IsEmailConfirmedAsync(user);

                if (!hasConfirmedEmail)
                {
                    string callbackUrl = await GetCallbackUrlAsync(user);

                    await SendRegistrationEmail(user.UserName, user.Email, callbackUrl);
                }
            }

            return Ok("Email was sent");
        }

        private async Task SendRegistrationEmail(string userName, string userEmail, string callbackUrl)
        {
            _senderName = _configuration.GetValue<string>("AdminFromEmail");
            var email = new SendEmail(userEmail,
                _senderName,
                "[Aplicatia de urgenta] Confirmare adresa email",// TODO: add I18n
            EmailTemplate.AccountConfirmation);

            email.PlaceholderContent.Add("name", HtmlEncoder.Default.Encode(userName));
            email.PlaceholderContent.Add("confirmationLink", callbackUrl);

            await _mediator.Publish(email);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            var badRegistrationResponse = new RegistrationResponse
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
