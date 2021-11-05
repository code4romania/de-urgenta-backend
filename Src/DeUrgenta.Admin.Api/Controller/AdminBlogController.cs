using System;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Admin.Api.Swagger.Blog;
using DeUrgenta.Common.Auth;
using DeUrgenta.Common.Extensions;
using DeUrgenta.Common.Models.Pagination;
using DeUrgenta.Common.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Controller
{
    [ApiController]
    [Authorize(Policy = ApiPolicies.AdminOnly)]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("admin/blog")]
    public class AdminBlogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminBlogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets blog posts
        /// </summary>
        /// <returns></returns>
        [HttpGet("posts")]
        [SwaggerResponse(StatusCodes.Status200OK, "Blog posts", typeof(PagedResult<BlogPostModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBlogPostsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<PagedResult<BlogPostModel>>> GetBlogPostsAsync(
            [FromQuery] PaginationQueryModel pagination)
        {
            var query = new GetBlogPosts(pagination);
            var result = await _mediator.Send(query);
            
            return result.ToActionResult();
        }

        /// <summary>
        /// Adds a new blog post
        /// </summary>
        /// <returns></returns>
        [HttpPost("post")]
        [SwaggerResponse(StatusCodes.Status200OK, "New blog post", typeof(BlogPostModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        [SwaggerRequestExample(typeof(BlogPostRequest), typeof(AddOrUpdateBlogPostRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateBlogPostResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BlogPostModel>> CreateNewBlogPostAsync([FromBody] BlogPostRequest blogPost)
        {
            var command = new CreateBlogPost(blogPost);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Updates a blog Post
        /// </summary>
        [HttpPut]
        [Route("post/{blogPostId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updated a blog post", typeof(BlogPostModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        [SwaggerRequestExample(typeof(BlogPostRequest), typeof(AddOrUpdateBlogPostRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateBlogPostResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BlogPostModel>> UpdateBlogPostAsync([FromRoute] Guid blogPostId,
            [FromBody] BlogPostRequest blogPost)
        {
            var command = new UpdateBlogPost(blogPostId, blogPost);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Delete a blog post
        /// </summary>
        [HttpDelete]
        [Route("post/{blogPostId:guid}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Blog post was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> DeleteBlogPostAsync([FromRoute] Guid blogPostId)
        {
            var command = new DeleteBlogPost(blogPostId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}