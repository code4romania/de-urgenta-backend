using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Immutable;
using System.Threading;
using DeUrgenta.Backpack.Api.Models;
using DeUrgenta.Backpack.Api.Swagger.Backpack;
using DeUrgenta.Common.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using DeUrgenta.Backpack.Api.Commands;
using DeUrgenta.Backpack.Api.Queries;
using DeUrgenta.Common.Controllers;
using DeUrgenta.Common.Mappers;

namespace DeUrgenta.Backpack.Api.Controllers
{
    [ApiController]
    [Route("backpack")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class BackpackController : BaseAuthController
    {
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public BackpackController(IMediator mediator, IResultMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets backpacks for current user in which he either is contributor or owner.
        /// </summary>
        [HttpGet("/backpacks")]
        [SwaggerResponse(StatusCodes.Status200OK, "User backpacks", typeof(IImmutableList<BackpackModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpacksResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackModel>>> GetBackpacksAsync(CancellationToken ct)
        {
            var query = new GetBackpacks(UserSub);
            var result = await _mediator.Send(query, ct);

            return await _mapper.MapToActionResult(result, ct);
        }

        /// <summary>
        /// Gets backpacks for current user for which he is owner.
        /// </summary>
        [HttpGet("/backpacks/my")]
        [SwaggerResponse(StatusCodes.Status200OK, "User backpacks", typeof(IImmutableList<BackpackModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpacksResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackModel>>> GetMyBackpacksAsync(CancellationToken ct)
        {
            var query = new GetMyBackpacks(UserSub);
            var result = await _mediator.Send(query, ct);

            return await _mapper.MapToActionResult(result, ct);
        }

        /// <summary>
        /// Adds a new backpack
        /// </summary>
        [HttpPost]

        [SwaggerResponse(StatusCodes.Status200OK, "New backpack", typeof(BackpackModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackModelRequest), typeof(AddOrUpdateBackpackRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateBackpackResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackModel>> CreateNewBackpackAsync([FromBody] BackpackModelRequest backpack
            , CancellationToken ct)
        {
            var command = new CreateBackpack(UserSub, backpack);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result, ct);
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
        public async Task<ActionResult<BackpackModel>> UpdateBackpackAsync([FromRoute] Guid backpackId, 
            [FromBody] BackpackModelRequest backpack,
            CancellationToken ct)
        {
            var command = new UpdateBackpack(UserSub, backpackId, backpack);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result, ct);
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
        public async Task<ActionResult<IImmutableList<BackpackContributorModel>>> GetBackpackContributorsAsync([FromRoute] Guid backpackId,
            CancellationToken ct)
        {
            var command = new GetBackpackContributors(UserSub, backpackId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result, ct);
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
        public async Task<IActionResult> RemoveContributorAsync([FromRoute] Guid backpackId, [FromRoute] Guid userId, CancellationToken ct)
        {
            var command = new RemoveContributor(UserSub, backpackId, userId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result, ct);
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
        public async Task<IActionResult> RemoveCurrentContributorAsync([FromRoute] Guid backpackId, CancellationToken ct)
        {
            var command = new RemoveCurrentUserFromContributors(UserSub, backpackId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result, ct);
        }

        /// <summary>
        /// Delete a backpack
        /// </summary>
        [HttpDelete]
        [Route("{backpackId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Backpack was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> DeleteBackpackAsync([FromRoute] Guid backpackId, CancellationToken ct)
        {
            var command = new DeleteBackpack(UserSub, backpackId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result, ct);
        }
    }
}
