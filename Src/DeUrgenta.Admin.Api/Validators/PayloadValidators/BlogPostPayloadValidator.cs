using DeUrgenta.Admin.Api.Models;
using FluentValidation;

namespace DeUrgenta.Admin.Api.Validators.PayloadValidators
{
    public class BlogPostPayloadValidator:AbstractValidator<BlogPostRequest>
    {
        public BlogPostPayloadValidator()
        {
            RuleFor(c => c.Author).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(c => c.Title).NotEmpty().MinimumLength(3).MaximumLength(250);
            RuleFor(c => c.ContentBody).NotEmpty();
        }
    }
}
