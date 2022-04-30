using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Common.Mappers;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("group")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class GroupSafeLocationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public GroupSafeLocationsController(IMediator mediator, IResultMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }


        /// <summary>
        /// Gets group safe location
        /// </summary>
        /// <returns></returns>
        [HttpGet("{groupId:guid}/safe-locations")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get safe locations of a group", typeof(IImmutableList<SafeLocationResponseModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupSafeLocationsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<SafeLocationResponseModel>>> GetGroupSafeLocationsAsync([FromRoute] Guid groupId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetGroupSafeLocations(sub, groupId);
            var result = await _mediator.Send(query);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Adds a new group safe location
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("{groupId:guid}/safe-location")]
        [SwaggerResponse(StatusCodes.Status200OK, "New group safe location", typeof(SafeLocationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(SafeLocationRequest), typeof(AddOrUpdateSafeLocationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateSafeLocationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<SafeLocationResponseModel>> CreateNewSafeLocationAsync([FromRoute] Guid groupId, [FromBody] SafeLocationRequest safeLocation)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new AddSafeLocation(sub, groupId, safeLocation);
            var result = await _mediator.Send(query);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Updates a group safe location
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("safe-location/{locationId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated group safe location", typeof(SafeLocationResponseModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(SafeLocationRequest), typeof(AddOrUpdateSafeLocationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateSafeLocationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<SafeLocationResponseModel>> UpdateSafeLocationAsync([FromRoute] Guid locationId, [FromBody] SafeLocationRequest safeLocation)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new UpdateSafeLocation(sub, locationId, safeLocation);
            var result = await _mediator.Send(query);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Delete a group safe location
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("safe-location/{locationId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Group safe location was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> DeleteSafeLocationAsync([FromRoute] Guid locationId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new DeleteSafeLocation(sub, locationId);
            var result = await _mediator.Send(command);

            return await _mapper.MapToActionResult(result);
        }
    }
}
