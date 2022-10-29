using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Controllers;
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
    public class GroupController : BaseAuthController
    {
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public GroupController(IMediator mediator, IResultMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets user groups
        /// </summary>d
        /// <returns></returns>
        [HttpGet("/groups")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get groups of a user", typeof(IImmutableList<GroupModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<GroupModel>>> GetGroupsAsync(CancellationToken ct)
        {
            var query = new GetMyGroups(UserSub);
            var result = await _mediator.Send(query, ct);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Gets groups administered by current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("/groups/my")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get groups of a user", typeof(IImmutableList<GroupModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<GroupModel>>> GetMyGroupsAsync(CancellationToken ct)
        {
            var query = new GetAdministeredGroups(UserSub);
            var result = await _mediator.Send(query, ct);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Adds a new group
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "New group", typeof(GroupModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(GroupRequest), typeof(AddOrUpdateGroupRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateGroupResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<GroupModel>> CreateNewGroupAsync([FromBody] GroupRequest group, CancellationToken ct)
        {
            var command = new AddGroup(UserSub, group);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Updates a group
        /// </summary>
        [HttpPut]
        [Route("{groupId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Updated group", typeof(GroupModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(GroupRequest), typeof(AddOrUpdateGroupRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateGroupResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<GroupModel>> UpdateGroupAsync([FromRoute] Guid groupId, 
            [FromBody] GroupRequest group,
            CancellationToken ct)
        {
            var command = new UpdateGroup(UserSub, groupId, group);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Gets a list of group members
        /// </summary>
        [HttpGet]
        [Route("{groupId:guid}/members")]
        [SwaggerResponse(StatusCodes.Status200OK, "List of group members", typeof(IImmutableList<GroupMemberModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupMembersResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<GroupMemberModel>>> GetGroupMembersAsync([FromRoute] Guid groupId, CancellationToken ct)
        {
            var query = new GetGroupMembers(UserSub, groupId);
            var result = await _mediator.Send(query, ct);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Removes a user from group members
        /// </summary>
        [HttpDelete]
        [Route("{groupId:guid}/member/{userId:guid}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User removed from members")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> RemoveMemberAsync([FromRoute] Guid groupId, [FromRoute] Guid userId, CancellationToken ct)
        {
            var command = new RemoveFromGroup(UserSub, groupId, userId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Removes current user from group members
        /// </summary>
        [HttpPut]
        [Route("{groupId:guid}/member/leave")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User removed from members")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> LeaveGroupAsync([FromRoute] Guid groupId, CancellationToken ct)
        {
            var command = new LeaveGroup(UserSub, groupId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{groupId:guid}")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Group was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> DeleteGroupAsync([FromRoute] Guid groupId, CancellationToken ct)
        {
            var command = new DeleteGroup(UserSub, groupId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result);
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
        public async Task<ActionResult<IImmutableList<SafeLocationResponseModel>>> GetGroupSafeLocationsAsync([FromRoute] Guid groupId,
            CancellationToken ct)
        {
            var query = new GetGroupSafeLocations(UserSub, groupId);
            var result = await _mediator.Send(query, ct);

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
        public async Task<ActionResult<SafeLocationResponseModel>> CreateNewSafeLocationAsync([FromRoute] Guid groupId,
            [FromBody] SafeLocationRequest safeLocation,
            CancellationToken ct)
        {
            var query = new AddSafeLocation(UserSub, groupId, safeLocation);
            var result = await _mediator.Send(query, ct);

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
        public async Task<ActionResult<SafeLocationResponseModel>> UpdateSafeLocationAsync([FromRoute] Guid locationId,
            [FromBody] SafeLocationRequest safeLocation, 
            CancellationToken ct)
        {
            var query = new UpdateSafeLocation(UserSub, locationId, safeLocation);
            var result = await _mediator.Send(query, ct);

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
        public async Task<IActionResult> DeleteSafeLocationAsync([FromRoute] Guid locationId, CancellationToken ct)
        {
            var command = new DeleteSafeLocation(UserSub, locationId);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result);
        }
    }
}
