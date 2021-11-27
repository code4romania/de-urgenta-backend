using System;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Common.Mappers;
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
    [Authorize]
    [Route("invite")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class InviteController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public InviteController(IMediator mediator, IResultMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Generate an invite to a group or backpack
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Accept an invite to a group or backpack 
        /// </summary>
        /// <param name="inviteId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{inviteId:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Accept invite ro group or backpack", typeof(AcceptInviteModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(Guid))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<AcceptInviteModel>> AcceptInvite([FromRoute] Guid inviteId)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var result = await _mediator.Send(new AcceptInvite(sub, inviteId));

            return await _mapper.MapToActionResult(result);
        }
    }
}
