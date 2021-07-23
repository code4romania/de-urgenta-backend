using DeUrgenta.Group.Api.Models;
using FluentValidation;

namespace DeUrgenta.Group.Api.Validators.RequestValidators
{
    public class GroupRequestValidator : AbstractValidator<GroupRequest>
    {
        public GroupRequestValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(3).MaximumLength(250);
        }
    }
}
