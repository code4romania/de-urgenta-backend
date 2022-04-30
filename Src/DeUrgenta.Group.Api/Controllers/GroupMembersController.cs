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
    public class GroupMembersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public GroupMembersController(IMediator mediator, IResultMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetGroupMembers(sub, groupId);
            var result = await _mediator.Send(query);

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
        public async Task<ActionResult> RemoveMemberAsync([FromRoute] Guid groupId, [FromRoute] Guid userId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new RemoveFromGroup(sub, groupId, userId);
            var result = await _mediator.Send(command);

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
        public async Task<IActionResult> LeaveGroupAsync([FromRoute] Guid groupId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new LeaveGroup(sub, groupId);
            var result = await _mediator.Send(command);

            return await _mapper.MapToActionResult(result);
        }


    }
}
