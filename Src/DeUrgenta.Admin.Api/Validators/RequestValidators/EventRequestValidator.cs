using System;
using DeUrgenta.Admin.Api.Models;
using FluentValidation;

namespace DeUrgenta.Admin.Api.Validators.RequestValidators
{
    public class EventRequestValidator : AbstractValidator<EventRequest>
    {
        public EventRequestValidator()
        {
            RuleFor(c => c.Title).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.OrganizedBy).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(c => c.ContentBody).NotEmpty();
            RuleFor(c => c.OccursOn).GreaterThanOrEqualTo(DateTime.Today);
            RuleFor(c => c.Author).NotEmpty().MinimumLength(3).MaximumLength(100);

            RuleFor(c => c.Address).NotEmpty();
            RuleFor(c => c.Locality).NotEmpty();
        }
    }
}
