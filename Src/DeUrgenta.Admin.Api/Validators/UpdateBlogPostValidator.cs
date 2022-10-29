using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class UpdateBlogPostValidator : IValidateRequest<UpdateBlogPost>
    {
        private readonly DeUrgentaContext _context;

        public UpdateBlogPostValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<ValidationResult> IsValidAsync(UpdateBlogPost request, CancellationToken ct)
        {
            var blogPostExists = await _context.Blogs.AnyAsync(x => x.Id == request.BlogPostId, ct);

            return blogPostExists
                ? ValidationResult.Ok
                : new LocalizableValidationError("blogpost-not-exist",new LocalizableString("blogpost-not-exist-message", request.BlogPostId));
        }
    }
}