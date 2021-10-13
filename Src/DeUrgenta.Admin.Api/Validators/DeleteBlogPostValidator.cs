using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class DeleteBlogPostValidator : IValidateRequest<DeleteBlogPost>
    {
        private readonly DeUrgentaContext _context;

        public DeleteBlogPostValidator(DeUrgentaContext context)
        {
            _context = context;
        }
        public async Task<ValidationResult> IsValidAsync(DeleteBlogPost request)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == request.BlogPostId);

            return blog is not null ? ValidationResult.Ok : ValidationResult.GenericValidationError;
        }
    }
}