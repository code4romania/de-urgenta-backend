using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class UpdateBlogPostHandler : IRequestHandler<UpdateBlogPost, Result<BlogPostModel, ValidationResult>>
    {
        private readonly IValidateRequest<UpdateBlogPost> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateBlogPostHandler(IValidateRequest<UpdateBlogPost> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<BlogPostModel, ValidationResult>> Handle(UpdateBlogPost request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var blogPost = await _context.Blogs.FirstAsync(x=>x.Id == request.BlogPostId, cancellationToken: cancellationToken);
            blogPost.Author = request.BlogPost.Author;
            blogPost.Title = request.BlogPost.Title;
            blogPost.ContentBody = request.BlogPost.ContentBody;
            blogPost.PublishedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            
            return new BlogPostModel
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Title = blogPost.Title,
                ContentBody = blogPost.ContentBody,
                PublishedOn = blogPost.PublishedOn
            };
        }
    }
}