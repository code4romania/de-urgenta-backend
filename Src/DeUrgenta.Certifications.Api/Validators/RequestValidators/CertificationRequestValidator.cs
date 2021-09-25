using System;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Common.Validation;
using FluentValidation;

namespace DeUrgenta.Certifications.Api.Validators.RequestValidators
{
    public class CertificationRequestValidator : AbstractValidator<CertificationRequest>
    {
        public CertificationRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.IssuingAuthority).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.ExpirationDate).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(c => c.Photo).SetValidator(new ImageFileValidator());
        }
    }
}
