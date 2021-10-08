using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;
using DeUrgenta.Invite.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Invite.Api.Controllers
{
    [Route("invite")]
    [ApiController]
    public class InviteController: ControllerBase
    {
        private readonly IMediator _mediator;

        public InviteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "Invite created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(InviteRequest), typeof(AddInviteRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<InviteModel>> GenerateInvite(InviteRequest request)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new CreateInvite(sub, request));

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }
    }
}
