﻿using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Backpack.Api.Swagger.BackpackItem;
using DeUrgenta.Common.Extensions;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Domain.Api.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Controllers
{
    [ApiController]
    [Route("backpack")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class BackpackItemController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BackpackItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets items in a backpack
        /// </summary>
        /// <returns></returns>
        [HttpGet("{backpackId:guid}/items")]
        [SwaggerResponse(StatusCodes.Status200OK, "Items from a backpack", typeof(IImmutableList<BackpackItemModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpackItemsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackItemModel>>> GetBackpackItemsAsync([FromRoute] Guid backpackId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetBackpackItems(sub, backpackId);
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        /// <summary>
        /// Gets items in a backpack for a specific category
        /// </summary>
        /// <returns></returns>
        [HttpGet("{backpackId:guid}/{categoryId:int}/items")]
        [SwaggerResponse(StatusCodes.Status200OK, "Items from a backpack category", typeof(IImmutableList<BackpackItemModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCategoryBackpackItemsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackItemModel>>> GetBackpackCategoryItemsAsync([FromRoute] Guid backpackId, [FromRoute] BackpackCategoryType categoryId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetBackpackCategoryItems(sub, backpackId, categoryId);
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        /// <summary>
        /// Adds a new backpack item
        /// </summary>
        /// <returns></returns>
        [HttpPost("{backpackId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "New backpack item", typeof(BackpackItemModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackItemRequest), typeof(AddBackpackItemRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddBackpackItemResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackItemModel>> CreateNewBackpackItemAsync([FromRoute] Guid backpackId, [FromBody] BackpackItemRequest backpackItem)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new AddBackpackItem(sub, backpackId, backpackItem);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Updates a backpack item
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("item/{itemId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated backpack item", typeof(BackpackItemModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackItemRequest), typeof(UpdateBackpackItemRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UpdateBackpackItemResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackItemModel>> UpdateBackpackItemAsync([FromRoute] Guid itemId, [FromBody] BackpackItemRequest backpackItem)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new UpdateBackpackItem(sub, itemId, backpackItem);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Delete a backpack item
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("item/{itemId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Backpack item was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> DeleteBackpackItemAsync([FromRoute] Guid itemId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new DeleteBackpackItem(sub, itemId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
