using System.Threading.Tasks;
using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Certifications.Api.Validators
{
    public class UpdateCertificationValidator : IValidateRequest<UpdateCertification>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public UpdateCertificationValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }
        public async Task<ValidationResult> IsValidAsync(UpdateCertification request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Sub == request.UserSub);
            if (user == null)
            {
                return ValidationResult.GenericValidationError;
            }

            var certificationExists = await _context.Certifications.AnyAsync(c => c.UserId == user.Id && c.Id == request.CertificationId);

            if (!certificationExists)
            {
                return new DetailedValidationError(await _i18nProvider.Localize("certification-not-exist"), await _i18nProvider.Localize("certification-not-exist-message"));
            }

            return ValidationResult.Ok;
        }
    }
}
