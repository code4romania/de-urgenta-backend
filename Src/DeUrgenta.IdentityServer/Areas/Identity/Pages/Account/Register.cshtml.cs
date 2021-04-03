﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using DeUrgenta.IdentityServer;
using DeUrgenta.IdentityServer.Services.Emailing;

namespace IdentityServer.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly PasswordValidationMessages _passwordValidationMessages;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            PasswordValidationMessages passwordValidationMessages,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _passwordValidationMessages = passwordValidationMessages;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [FromQuery]
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel : IValidatableObject
        {
            [Required(ErrorMessage = "Te rugăm completează adresa de e-mail")]
            [EmailAddress(ErrorMessage = "Adresa de e-mail nu este validă.")]
            [Display(Name = "Adresa de e-mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Te rugăm să completezi parola")]
            [StringLength(100, ErrorMessage = "Parola trebuie să aibă lungimea între minim {2} și maximum {1} caractere.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Setează parola")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Te rugăm să completezi confirmarea parolei")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmă parola")]
            [Compare("Password", ErrorMessage = "Câmpurile de parolă și confirmare parolă nu sunt identice.")]
            public string ConfirmPassword { get; set; }
            public bool AcceptTermsAndConditions { get; set; }
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (AcceptTermsAndConditions == false)
                {
                    yield return new ValidationResult(
                        "Te rugăm să accepți termenii și condițiile.",
                        new[] { nameof(AcceptTermsAndConditions) });
                }
            }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        private async Task SendRegistrationEmail(string userName, string callbackUrl)
        {
            EmailRequestModel email = new EmailRequestModel
            {
                Address = Input.Email,
                PlaceholderContent = new Dictionary<string, string>(),
                TemplateType = EmailTemplate.AccountConfirmation,
                SenderName = "Admin De Urgenta",
                Subject = ""
            };
            email.PlaceholderContent.Add("name", HtmlEncoder.Default.Encode(userName));
            email.PlaceholderContent.Add("confirmationLink", callbackUrl);

            await _emailSender.SendAsync(email);
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Id, Input.Email),
                        new Claim(JwtClaimTypes.Email, Input.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    });
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await SendRegistrationEmail(user.UserName, callbackUrl);

                    return RedirectToPage("RegisterConfirmation");
                }

                if (result.Errors.Any(c => c.Code == "DuplicateUserName"))
                {
                    _logger.LogWarning("User already exits, redirect to register confirmation screen to avoid user enumeration.");
                    return RedirectToPage("RegisterConfirmation");
                }

                foreach (var error in result.Errors)
                {
                    var errorDescription = _passwordValidationMessages.GetMessageByCode(error);
                    ModelState.AddModelError(string.Empty, errorDescription);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
