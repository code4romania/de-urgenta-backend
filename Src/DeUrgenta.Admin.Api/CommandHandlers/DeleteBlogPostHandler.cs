using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPost, Result<Unit, ValidationResult>>
    {
        private readonly IValidateRequest<DeleteBlogPost> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteBlogPostHandler(IValidateRequest<DeleteBlogPost> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Unit, ValidationResult>> Handle(DeleteBlogPost request, CancellationToken ct)
        {
            var validationResult = await _validator.IsValidAsync(request, ct);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var blogPostToBeDeleted = await _context.Blogs.FirstAsync(x => x.Id == request.BlogPostId, ct);
            _context.Remove(blogPostToBeDeleted);
            await _context.SaveChangesAsync(ct);
            
            return Unit.Value;
        }
    }
}