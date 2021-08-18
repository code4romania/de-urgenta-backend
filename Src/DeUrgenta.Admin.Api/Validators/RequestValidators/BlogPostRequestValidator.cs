using DeUrgenta.Admin.Api.Models;
using FluentValidation;

namespace DeUrgenta.Admin.Api.Validators.RequestValidators
{
    public class BlogPostRequestValidator : AbstractValidator<BlogPostRequest>
    {
        public BlogPostRequestValidator()
        {
            RuleFor(c => c.Author).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(c => c.Title).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.ContentBody).NotEmpty();
        }
    }
}
