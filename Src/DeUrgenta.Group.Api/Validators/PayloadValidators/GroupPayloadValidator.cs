using DeUrgenta.Group.Api.Models;
using FluentValidation;

namespace DeUrgenta.Group.Api.Validators.PayloadValidators
{
    public class GroupPayloadValidator : AbstractValidator<GroupRequest>
    {
        public GroupPayloadValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
        }
    }
}
