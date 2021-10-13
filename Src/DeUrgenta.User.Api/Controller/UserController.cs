using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Common.Extensions;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Infra.Models;
using DeUrgenta.User.Api.Commands;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Models.DTOs.Responses;
using DeUrgenta.User.Api.Queries;
using DeUrgenta.User.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Controller
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user info about current user
        /// </summary>
        /// <returns></returns>
        [HttpGet("whoami")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get details about an user", typeof(UserModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserModel>> GetUserDetailsAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new GetUser(sub));

            return result.ToActionResult();
        }

        /// <summary>
        /// Updates user info
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Update successful")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(UserRequest), typeof(AddOrUpdateUserRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserModel>> UpdateUserAsync(UserRequest user)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new UpdateUser(sub, user));

            return result.ToActionResult();
        }


        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequest userChangePassword)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return BadRequest("Invalid User");
            }

            var correctPassword = await _userManager.CheckPasswordAsync(user, userChangePassword.OldPassword);



            var resp = await _userManager.ChangePasswordAsync(user, userChangePassword.OldPassword, userChangePassword.NewPassword);

            if (!resp.Succeeded)
            {
                return BadRequest(
                    new ActionResponse
                    {
                        Errors = resp.Errors.Select(e => e.Description).ToList(),
                        Success = false
                    }
                );
            }

            return Ok("Password changed");


        }

        /// <summary>
        /// Gets user location types
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("location-types")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get available location types", typeof(IImmutableList<IndexedItemModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserLocationTypesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackInviteModel>> GetUserLocationTypesAsync()
        {
            var query = new GetUserLocationTypes();
            var locationTypes = await _mediator.Send(query);

            return Ok(locationTypes);
        }

        /// <summary>
        /// Gets user locations
        /// </summary>
        /// <returns></returns>
        [HttpGet("locations")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get locations of current user", typeof(IImmutableList<UserLocationModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserLocationsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<UserLocationModel>>> GetUserLocationsAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new GetUserLocations(sub));

            return result.ToActionResult();
        }

        /// <summary>
        /// Adds a new user location
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("location")]
        [SwaggerResponse(StatusCodes.Status200OK, "new user location", typeof(UserLocationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(UserLocationRequest), typeof(AddOrUpdateUserLocationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddUserLocationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserLocationModel>> AddLocationAsync([FromBody] UserLocationRequest location)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new AddLocation(sub, location));

            return result.ToActionResult();
        }

        /// <summary>
        /// Updates user location
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("location/{locationId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Updated user location")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(UserLocationRequest), typeof(AddOrUpdateUserLocationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserLocationModel>> UpdateLocationAsync([FromRoute] Guid locationId, [FromBody] UserLocationRequest location)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new UpdateLocation(sub, locationId, location));

            return result.ToActionResult();
        }

        /// <summary>
        /// Delete an user location
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("location/{locationId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "User location was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> DeleteLocationAsync([FromRoute] Guid locationId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new DeleteLocation(sub, locationId));

            return result.ToActionResult();
        }


        /// <summary>
        /// Gets group invites
        /// </summary>
        /// <returns></returns>
        [HttpGet("group-invites")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get group invites for current user", typeof(IImmutableList<GroupInviteModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupInvitesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<GroupInviteModel>> GetGroupInvitesAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetGroupInvites(sub);
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        /// <summary>
        /// Accept a group invite
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("group-invite/{groupInviteId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Group invite accepted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> AcceptGroupInviteAsync([FromRoute] Guid groupInviteId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new AcceptGroupInvite(sub, groupInviteId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Reject a group invite
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("group-invite/{groupInviteId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Group invite rejected")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> RejectGroupInviteAsync([FromRoute] Guid groupInviteId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new RejectGroupInvite(sub, groupInviteId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Gets backpack invites
        /// </summary>
        /// <returns></returns>
        [HttpGet("backpack-invites")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get backpack invites for current user", typeof(IImmutableList<BackpackInviteModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetGroupInvitesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackInviteModel>> GetBackpackInvitesAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetBackpackInvites(sub);
            var result = await _mediator.Send(query);

            return result.ToActionResult();
        }

        /// <summary>
        /// Accept a backpack invite
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("backpack-invite/{backpackInviteId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Group invite accepted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> AcceptBackpackInviteAsync([FromRoute] Guid backpackInviteId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new AcceptBackpackInvite(sub, backpackInviteId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }

        /// <summary>
        /// Reject a backpack invite
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("backpack-invite/{backpackInviteId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Group invite rejected")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<IActionResult> RejectBackpackInviteAsync([FromRoute] Guid backpackInviteId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new RejectBackpackInvite(sub, backpackInviteId);
            var result = await _mediator.Send(command);

            return result.ToActionResult();
        }
    }
}
