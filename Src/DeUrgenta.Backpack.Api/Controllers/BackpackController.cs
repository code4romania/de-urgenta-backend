using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using System.Collections.Immutable;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Swagger.Backpack;
using DeUrgenta.Common.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Linq;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Extensions;

namespace DeUrgenta.Backpack.Api.Controllers
{
    [ApiController]
    [Route("backpack")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class BackpackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BackpackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets backpacks for current user in which he either is contributor or owner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/backpacks")]
        [SwaggerResponse(StatusCodes.Status200OK, "User backpacks", typeof(IImmutableList<BackpackModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpacksResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackModel>>> GetBackpacksAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetBackpacks(sub);
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        /// <summary>
        /// Gets backpacks for current user for which he is owner.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/backpacks/my")]
        [SwaggerResponse(StatusCodes.Status200OK, "User backpacks", typeof(IImmutableList<BackpackModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpacksResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackModel>>> GetMyBackpacksAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetMyBackpacks(sub);
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        /// <summary>
        /// Adds a new backpack
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        [SwaggerResponse(StatusCodes.Status200OK, "New backpack", typeof(BackpackModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackModelRequest), typeof(AddOrUpdateBackpackRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateBackpackResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackModel>> CreateNewBackpackAsync([FromBody] BackpackModelRequest backpack)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var command = new CreateBackpack(sub, backpack);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Updates a backpack
        /// </summary>
        [HttpPut]
        [Route("{backpackId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated backpack", typeof(BackpackModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackModelRequest), typeof(AddOrUpdateBackpackRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateBackpackResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackModel>> UpdateBackpackAsync([FromRoute] Guid backpackId, [FromBody] BackpackModelRequest backpack)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var command = new UpdateBackpack(sub, backpackId, backpack);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Gets a list of backpack contributors
        /// </summary>
        [HttpGet]
        [Route("{backpackId:guid}/contributors")]

        [SwaggerResponse(StatusCodes.Status200OK, "List of contributors", typeof(IImmutableList<BackpackContributorModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpackContributorsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackContributorModel>>> GetBackpackContributorsAsync([FromRoute] Guid backpackId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var command = new GetBackpackContributors(sub, backpackId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>

            return NoContent();
        }

        /// <summary>
        /// Removes a user from contributors of a backpack
        /// </summary>
        [HttpDelete]
        [Route("{backpackId:guid}/contributor/{userId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "User removed from contributors")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackModelRequest), typeof(AddOrUpdateBackpackRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> RemoveContributorAsync([FromRoute] Guid backpackId, [FromRoute] Guid userId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var command = new RemoveContributor(sub, backpackId, userId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Removes current user from contributors of a backpack
        /// </summary>
        [HttpPut]
        [Route("{backpackId:guid}/contributor/leave")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "User leaved from contributors")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> RemoveCurrentContributorAsync([FromRoute] Guid backpackId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var command = new RemoveCurrentUserFromContributors(sub, backpackId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Delete a backpack
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{backpackId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Backpack was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> DeleteBackpackAsync([FromRoute] Guid backpackId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var command = new DeleteBackpack(sub, backpackId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
