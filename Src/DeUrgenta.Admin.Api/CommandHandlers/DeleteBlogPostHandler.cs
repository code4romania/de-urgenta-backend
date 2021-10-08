using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPost, Result>
    {
        private readonly IValidateRequest<DeleteBlogPost> _validator;
        private readonly DeUrgentaContext _context;

        public DeleteBlogPostHandler(IValidateRequest<DeleteBlogPost> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(DeleteBlogPost request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var blogPostToBeDeleted =
                await _context.Blogs.FirstAsync(x => x.Id == request.BlogPostId, cancellationToken);
            _context.Remove(blogPostToBeDeleted);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}