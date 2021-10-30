using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.Validators
{
    public class DeleteBlogPostValidator : IValidateRequest<DeleteBlogPost>
    {
        private readonly DeUrgentaContext _context;
        private readonly IamI18nProvider _i18nProvider;

        public DeleteBlogPostValidator(DeUrgentaContext context, IamI18nProvider i18nProvider)
        {
            _context = context;
            _i18nProvider = i18nProvider;
        }
        public async Task<ValidationResult> IsValidAsync(DeleteBlogPost request)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == request.BlogPostId);

            return blog is not null 
                ? ValidationResult.Ok 
                : new DetailedValidationError(await _i18nProvider.Localize("blogpost-not-exist"), await _i18nProvider.Localize("blogpost-not-exist-message", request.BlogPostId));
        }
    }
}