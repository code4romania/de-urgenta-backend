using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Common.Swagger;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using DeUrgenta.User.Api.Models.DTOs.Responses;
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
        /// Gets user info
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Get details about an user", typeof(UserModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserModel>> GetUserDetailsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new group
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "New user", typeof(UserModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(UserRequest), typeof(AddOrUpdateUserRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateUserResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserModel>> UpdateUserAsync(UserRequest user)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody]UserChangePassword userChangePassword){
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            var user = await _userManager.FindByEmailAsync(userEmail);

            if(user==null){
                return BadRequest("Invalid User");
            }

            var correctPassword = await _userManager.CheckPasswordAsync(user, userChangePassword.OldPassword);

            

            var resp = await _userManager.ChangePasswordAsync(user, userChangePassword.OldPassword, userChangePassword.NewPassword);

            if(!resp.Succeeded){
                return BadRequest(
                    new ActionResponse
                    {
                        Errors = resp.Errors.Select(e=>e.Description).ToList(),
                        Success = false
                    }
                );
            }

            return Ok("Password changed");


        }

        /// <summary>
        /// Gets user safe location
        /// </summary>
        /// <returns></returns>
        [HttpGet("safe-locations")]
        [SwaggerResponse(StatusCodes.Status200OK, "Get safe locations of current user", typeof(IImmutableList<UserSafeLocationModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserSafeLocationsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<UserSafeLocationModel>>> GetUserSafeLocationsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new user safe location
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("safe-location")]
        [SwaggerResponse(StatusCodes.Status200OK, "new user safe location", typeof(UserSafeLocationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(UserSafeLocationRequest), typeof(AddOrUpdateUserSafeLocationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddUserSafeLocationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserSafeLocationModel>> CreateNewSafeLocationAsync([FromBody] UserSafeLocationRequest safeLocation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates user safe location
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("safe-location/{locationId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated user safe location", typeof(UserSafeLocationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(UserSafeLocationRequest), typeof(AddOrUpdateUserSafeLocationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddUserSafeLocationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<UserSafeLocationModel>> UpdateSafeLocationAsync([FromRoute] Guid locationId, [FromBody] UserSafeLocationRequest safeLocation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete an user safe location
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("safe-location/{locationId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "User safe location was deleted")]
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
