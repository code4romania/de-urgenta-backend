using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Domain.Entities;
using MediatR;

namespace DeUrgenta.Admin.Api.CommandHandlers
{
    public class CreateBlogPostHandler : IRequestHandler<CreateBlogPost, Result<BlogPostModel, ValidationResult>>
    {
        private readonly IValidateRequest<CreateBlogPost> _validator;
        private readonly DeUrgentaContext _context;

        public CreateBlogPostHandler(IValidateRequest<CreateBlogPost> validator,DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }
        public async Task<Result<BlogPostModel, ValidationResult>> Handle(CreateBlogPost request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var blogPost = new BlogPost()
            {
                Author = request.Blog.Author,
                Title = request.Blog.Title,
                ContentBody = request.Blog.ContentBody,
                PublishedOn = DateTime.UtcNow
            };

            await _context.AddAsync(blogPost, cancellationToken);
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