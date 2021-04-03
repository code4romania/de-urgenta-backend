using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Models;
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
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets upcoming events
        /// </summary>
        /// <returns></returns>
        [HttpGet("events")]
        [AllowAnonymous]

        [SwaggerResponse(StatusCodes.Status200OK, "Upcoming events", typeof(PagedResult<EventModel>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<PagedResult<EventModel>>> GetEventsAsync([FromQuery] PaginationQueryModel pagination)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new event
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "New event", typeof(EventModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(EventRequest), typeof(AddOrUpdateEventRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateEventResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<EventModel>> CreateNewEventAsync([FromBody] EventRequest eventModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a event
        /// </summary>
        [HttpPut]
        [Route("{eventId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated event", typeof(EventModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(EventRequest), typeof(AddOrUpdateEventRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateEventResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<EventModel>> UpdateEventAsync([FromRoute] Guid eventId, [FromBody] EventRequest eventModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete a event
        /// </summary>
        [HttpDelete]
        [Route("{eventId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Event was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "A non authorized request was made")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> DeleteEventAsync([FromRoute] Guid eventId)
        {
            throw new NotImplementedException();
        }
    }
}
