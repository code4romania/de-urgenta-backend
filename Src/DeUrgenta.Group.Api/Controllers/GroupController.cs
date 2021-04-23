﻿using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Controllers
{
    [ApiController]
    [Route("group")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user groups
        /// </summary>
        /// <returns></returns>
        [HttpGet("/groups")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get groups of a user", typeof(IImmutableList<GroupModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<GroupModel>>> GetGroupsAsync()
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult<IImmutableList<GroupModel>>> GetMyGroupsAsync()
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult<GroupModel>> CreateNewGroupAsync([FromBody] GroupRequest group)
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult<GroupModel>> UpdateGroupAsync([FromRoute] Guid groupId, [FromBody] GroupRequest group)
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult<IImmutableList<GroupMemberModel>>> GetGroupMembersAsync([FromRoute] Guid groupId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invites a user as member to a group
        /// </summary>
        [HttpPut]
        [Route("{groupId:guid}/member/{userId:guid}/invite")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Invitation sent")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupMembersResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> AddMemberAsync([FromRoute] Guid groupId, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult> RemoveMemberAsync([FromRoute] Guid groupId, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult> LeaveGroupAsync([FromRoute] Guid groupId)
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult> DeleteGroupAsync([FromRoute] Guid groupId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets group safe location
        /// </summary>
        /// <returns></returns>
        [HttpGet("{groupId:guid}/safe-locations")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get safe locations of a group", typeof(IImmutableList<SafeLocationModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupSafeLocationsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<SafeLocationModel>>> GetGroupSafeLocationsAsync([FromRoute] Guid groupId)
        {
            throw new NotImplementedException();
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
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddSafeLocationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<SafeLocationModel>> CreateNewSafeLocationAsync([FromRoute] Guid groupId, [FromBody] SafeLocationRequest safeLocation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a group safe location
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{groupId:guid}/safe-location/{locationId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated group safe location", typeof(SafeLocationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(SafeLocationRequest), typeof(AddOrUpdateSafeLocationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddSafeLocationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<SafeLocationModel>> UpdateSafeLocationAsync([FromRoute] Guid groupId, [FromRoute] Guid locationId, [FromBody] SafeLocationRequest safeLocation)
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult> DeleteSafeLocationAsync([FromRoute] Guid locationId)
        {
            throw new NotImplementedException();
        }
    }
}
