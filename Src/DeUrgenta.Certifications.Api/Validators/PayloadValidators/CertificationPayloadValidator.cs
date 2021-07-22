using System;
using DeUrgenta.Certifications.Api.Models;
using FluentValidation;

namespace DeUrgenta.Certifications.Api.Validators.PayloadValidators
{
    public class CertificationPayloadValidator:AbstractValidator<CertificationRequest>
    {
        public CertificationPayloadValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.IssuingAuthority).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.ExpirationDate).GreaterThanOrEqualTo(DateTime.Today);
        }
    }
}
