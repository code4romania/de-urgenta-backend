using System;
using DeUrgenta.Certifications.Api.Models;
using FluentValidation;

namespace DeUrgenta.Certifications.Api.PayloadValidators
{
    public class CertificationPayloadValidator : AbstractValidator<CertificationRequest>
    {
        public CertificationPayloadValidator()
        {
            RuleFor(c => c.ExpirationDate).NotEmpty().GreaterThanOrEqualTo(DateTime.Today);

        }
    }
}
