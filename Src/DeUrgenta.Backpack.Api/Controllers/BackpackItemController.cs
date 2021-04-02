using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Swagger.BackpackItem;
using DeUrgenta.Common.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Controllers
{
    [ApiController]
    [Route("backpack-item/{backpackId:guid}/{categoryId:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BackpackItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BackpackItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets items of a backpack item
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Items from a backpack category", typeof(IImmutableList<BackpackItemModel>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpackItemsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackItemModel>>> GetBackpackItemsAsync([FromRoute] Guid backpackId, [FromRoute] Guid categoryId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new backpack item
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        [SwaggerResponse(StatusCodes.Status200OK, "New backpack item", typeof(BackpackItemModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackItemRequest), typeof(AddOrUpdateBackpackItemRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddBackpackItemResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackItemModel>> CreateNewBackpackItemAsync([FromRoute] Guid backpackId, [FromBody] BackpackItemRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a backpack item
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{itemId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated backpack item", typeof(BackpackItemModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackItemRequest), typeof(AddOrUpdateBackpackItemRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddBackpackItemResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackItemModel>> UpdateBackpackItemAsync([FromRoute] Guid backpackId, [FromRoute] Guid categoryId, [FromRoute] Guid itemId, [FromBody] BackpackItemRequest backpack)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a backpack item
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{itemId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Backpack item was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> DeleteBackpackItemAsync([FromRoute] Guid backpackId, [FromRoute] Guid categoryId, [FromRoute] Guid itemId)
        {
            throw new NotImplementedException();
        }
    }
}