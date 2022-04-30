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
    public class GroupController : ControllerBase
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
        public async Task<ActionResult<IImmutableList<GroupModel>>> GetGroupsAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetMyGroups(sub);
            var result = await _mediator.Send(query);

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
        public async Task<ActionResult<IImmutableList<GroupModel>>> GetMyGroupsAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetAdministeredGroups(sub);
            var result = await _mediator.Send(query);

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
        public async Task<ActionResult<GroupModel>> CreateNewGroupAsync([FromBody] GroupRequest group)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new AddGroup(sub, group);
            var result = await _mediator.Send(command);

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
        public async Task<ActionResult<GroupModel>> UpdateGroupAsync([FromRoute] Guid groupId, [FromBody] GroupRequest group)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new UpdateGroup(sub, groupId, group);
            var result = await _mediator.Send(command);

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
        public async Task<IActionResult> DeleteGroupAsync([FromRoute] Guid groupId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new DeleteGroup(sub, groupId);
            var result = await _mediator.Send(command);

            return await _mapper.MapToActionResult(result);
        }
    }
}
