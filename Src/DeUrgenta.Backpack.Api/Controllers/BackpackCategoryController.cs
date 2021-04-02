using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Swagger.BackpackCategory;
using DeUrgenta.Common.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Controllers
{
    [ApiController]
    [Route("backpack-category/{backpackId:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BackpackCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BackpackCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets categories of a backpack
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [SwaggerResponse(StatusCodes.Status200OK, "A list of categories for a backpack", typeof(IImmutableList<BackpackCategoryModel>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made", typeof(void))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpackCategoriesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackCategoryModel>>> GetBackpackCategoriesAsync([FromRoute] Guid backpackId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new backpack category
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Created category for a backpack", typeof(BackpackCategoryModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackCategoryRequest), typeof(AddOrUpdateBackpackCategoryRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateBackpackCategoryResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackCategoryModel>> CreateNewBackpackCategoryAsync([FromRoute] Guid backpackId, [FromBody] BackpackCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a backpack category
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{categoryId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updated category for a backpack", typeof(BackpackCategoryModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackCategoryRequest), typeof(AddOrUpdateBackpackCategoryRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateBackpackCategoryResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackCategoryModel>> UpdateBackpackAsync([FromRoute] Guid backpackId, [FromRoute] Guid categoryId, [FromBody] BackpackCategoryRequest backpack)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a backpack category
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{categoryId:guid}")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> DeleteBackpackAsync([FromRoute] Guid backpackId, [FromRoute] Guid categoryId)
        {
            throw new NotImplementedException();
        }
    }
}