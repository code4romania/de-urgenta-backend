using System;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Queries;
using DeUrgenta.Admin.Api.Swagger.Events;
using DeUrgenta.Common.Models;
using DeUrgenta.Common.Swagger;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("event")]
    public class AdminEventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets upcoming events
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        [AllowAnonymous]

        [SwaggerResponse(StatusCodes.Status200OK, "Upcoming events", typeof(PagedResult<EventModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<PagedResult<EventModel>>> GetEventsAsync([FromQuery] PaginationQueryModel pagination)
        {
            var query = new GetEvents();
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Adds a new event
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "New event", typeof(EventModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(EventRequest), typeof(AddOrUpdateEventRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateEventResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<EventModel>> CreateNewEventAsync([FromBody] EventRequest eventModel)
        {
            var command = new CreateEvent(eventModel);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Updates a event
        /// </summary>
        [HttpPut]
        [Route("{eventId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated event", typeof(EventModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(EventRequest), typeof(AddOrUpdateEventRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateEventResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<EventModel>> UpdateEventAsync([FromRoute] Guid eventId, [FromBody] EventRequest eventModel)
        {
            var command = new UpdateEvent(eventId, eventModel);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a event
        /// </summary>
        [HttpDelete]
        [Route("{eventId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Event was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> DeleteEventAsync([FromRoute] Guid eventId)
        {
            var command = new DeleteEvent(eventId);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
