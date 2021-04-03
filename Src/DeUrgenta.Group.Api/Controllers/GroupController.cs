using System;
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
        /// Adds a new group
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        [SwaggerResponse(StatusCodes.Status200OK, "New group", typeof(GroupModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(GroupModelRequest), typeof(AddOrUpdateGroupRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateGroupResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<GroupModel>> CreateNewGroupAsync([FromBody] GroupModelRequest group)
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
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(GroupModelRequest), typeof(AddOrUpdateGroupRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateGroupResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<GroupModel>> UpdateGroupAsync([FromRoute] Guid groupId, [FromBody] GroupModelRequest group)
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
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made", typeof(ProblemDetails))]
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

        [SwaggerResponse(StatusCodes.Status204NoContent, "Invitation sent", typeof(GroupModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(GroupModelRequest), typeof(AddOrUpdateGroupRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupMembersResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<GroupModel>> AddMemberAsync([FromRoute] Guid groupId, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a user from group members
        /// </summary>
        [HttpDelete]
        [Route("{groupId:guid}/member/{userId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "User removed from members", typeof(GroupModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<GroupModel>> RemoveMemberAsync([FromRoute] Guid groupId, [FromRoute] Guid userId)
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
    }
}
