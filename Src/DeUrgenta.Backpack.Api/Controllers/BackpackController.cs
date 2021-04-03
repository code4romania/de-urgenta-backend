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

namespace DeUrgenta.Backpack.Api.Controllers
{
    [ApiController]
    [Route("backpack")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(AuthenticationSchemes = "backpackApiAuthenticationScheme")]
    public class BackpackController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BackpackController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user backpacks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "User backpacks", typeof(IImmutableList<BackpackModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpacksResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<BackpackModel>>> GetBackpacksAsync()
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult<BackpackModel>> CreateNewBackpackAsync([FromBody] BackpackModelRequest request)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            return Unauthorized();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invites a user to contribute to a backpack
        /// </summary>
        [HttpPut]
        [Route("{backpackId:guid}/contributor/{userId:guid}/invite")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Invitation sent", typeof(BackpackModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackModelRequest), typeof(AddOrUpdateBackpackRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetBackpackContributorsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackModel>> AddContributorAsync([FromRoute] Guid backpackId, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a user from contributors of a backpack
        /// </summary>
        [HttpDelete]
        [Route("{backpackId:guid}/contributor/{userId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "User removed from contributors", typeof(BackpackModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(BackpackModelRequest), typeof(AddOrUpdateBackpackRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<BackpackModel>> RemoveContributorAsync([FromRoute] Guid backpackId, [FromRoute] Guid userId)
        {
            throw new NotImplementedException();
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
        public async Task<ActionResult> DeleteBackpackAsync([FromRoute] Guid backpackId)
        {
            throw new NotImplementedException();
        }
    }
}
