using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DeUrgenta.Common.Models.Dtos;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Emailing.Service.Models;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Notifications;
using DeUrgenta.User.Api.Queries;
using DeUrgenta.User.Api.Services;
using DeUrgenta.User.Api.Swagger.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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

        /// <summary>
        /// Tries to create a new user account
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, "User account creation confirmation message")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Email already used", typeof(ActionResponse))]
        [SwaggerResponse(StatusCodes.Status429TooManyRequests, "Too many requests", typeof(ActionResponse))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuthRegisterExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ConflictErrorResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status429TooManyRequests, typeof(TooManyRequestsResponseExample))]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegistrationDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                //TODO: add I18n
                //TODO: consider returning a 200 with a more complex object as indicated here
                //https://stackoverflow.com/a/53144807/2780791
                return Conflict(new ActionResponse
                {
                    Success = false, 
                    Errors = new List<string> { "Adresa de e-mail este deja utilizată" }
                });
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

            return BadRequest(new ActionResponse
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

        private async Task<string> GetResetPasswordCallbakUrlAsync(IdentityUser user)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
            var resetUrl = _configuration.GetValue<string>("ResetUrl");
            var callbackUrl = $"{resetUrl}?userId={user.Id}&reset={resetToken}";
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
            var email = new SendEmail(userEmail,
                "[Aplicatia de urgenta] Confirmare adresa email",// TODO: add I18n
            EmailTemplate.AccountConfirmation);

            email.PlaceholderContent.Add("name", HtmlEncoder.Default.Encode(userName));
            email.PlaceholderContent.Add("confirmationLink", callbackUrl);

            await _mediator.Publish(email);
        }

        private async Task SendResetPasswordEmail(string userName, string userEmail, string callbackUrl)
        {
            var email = new SendEmail(userEmail,
              "[Aplicatia de urgenta] Resetare parolă",// TODO: add I18n
          EmailTemplate.ResetPassword);

            email.PlaceholderContent.Add("name", HtmlEncoder.Default.Encode(userName));
            email.PlaceholderContent.Add("resetPasswordLink", callbackUrl);

            await _mediator.Publish(email);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            var badRegistrationResponse = new ActionResponse
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
            var userDetails = await _mediator.Send(new GetUser(existingUser.Id));

            if (userDetails.IsFailure)
            {
                return BadRequest();
            }

            return Ok(new
            {
                token = jwtToken,
                userDetails.Value.LastName,
                userDetails.Value.FirstName,
                Success = true
            });
        }

        [HttpPost]
        [Route("request-reset-password")]
        public async Task<IActionResult> RequestResetPassword([FromBody] UserEmailPasswordResetRequest changePasswordRequest)
        {

            var user = await _userManager.FindByEmailAsync(changePasswordRequest.Email);

            if (user == null)
            {
                return NoContent();
            }
            var resetUrl = await this.GetResetPasswordCallbakUrlAsync(user);

            await SendResetPasswordEmail(user.UserName, user.Email, resetUrl);

            return NoContent();
        }


        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> RequestResetPassword([FromBody] UserResetPasswordRequest userResetPassword)
        {

            var user = await _userManager.FindByIdAsync(userResetPassword.UserId);
            var resToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(userResetPassword.ResetToken));
            var res = await _userManager.ResetPasswordAsync(user,resToken, userResetPassword.NewPassword);



            if (!res.Succeeded)
            {
                return BadRequest(new ActionResponse
                {
                    Errors = res.Errors.Select(x => x.Description).ToList(),
                    Success = false
                });
            }

            return Ok();

        }
    }
}