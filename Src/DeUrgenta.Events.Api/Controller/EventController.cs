using DeUrgenta.Events.Api.Models;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Mappers;
using DeUrgenta.Common.Swagger;
using DeUrgenta.Common.Models.Events;

namespace DeUrgenta.Events.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("event")]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public EventController(IMediator mediator, IResultMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets event types
        /// </summary>
        [HttpGet]
        [Route("types")]

        [SwaggerResponse(StatusCodes.Status200OK, "Event types", typeof(IImmutableList<EventTypeModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventTypesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<EventTypeModel>>> GetEventTypesAsync(CancellationToken ct)
        {
            var query = new GetEventTypes();
            var result = await _mediator.Send(query, ct);

            return await _mapper.MapToActionResult(result, ct);
        }

        /// <summary>
        /// Gets event cities
        /// </summary>
        [HttpGet]
        [Route("cities")]

        [SwaggerResponse(StatusCodes.Status200OK, "Event cities", typeof(IImmutableList<EventCityModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventCitiesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<EventCityModel>>> GetEventCitiesAsync([FromQuery] int? eventTypeId, CancellationToken ct)
        {
            var query = new GetEventCities(eventTypeId);
            var result = await _mediator.Send(query, ct);

            return await _mapper.MapToActionResult(result, ct);
        }

        /// <summary>
        /// Gets events list by city and/or type id
        /// </summary>
        [HttpGet]
        [Route("/events")]

        [SwaggerResponse(StatusCodes.Status200OK, "Events list", typeof(IImmutableList<EventResponseModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<EventResponseModel>>> GetEventsAsync([FromQuery] EventModelRequest filter, CancellationToken ct)
        {
            var command = new GetEvent(filter);
            var result = await _mediator.Send(command, ct);

            return await _mapper.MapToActionResult(result, ct);
        }
    }
}
