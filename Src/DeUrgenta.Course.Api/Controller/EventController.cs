using DeUrgenta.Events.Api.Models;
using DeUrgenta.Events.Api.Queries;
using DeUrgenta.Events.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Common.Swagger;
using Microsoft.AspNetCore.Authorization;
using DeUrgenta.Common.Models;

namespace DeUrgenta.Events.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("event")]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets event types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]

        [SwaggerResponse(StatusCodes.Status200OK, "Event types", typeof(IImmutableList<EventTypeModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventTypesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<EventTypeModel>>> GetEventTypesAsync()
        {
            var query = new GetEventTypes();
            var result = await _mediator.Send(query);
            
            if (result.IsFailure)
            {
                return BadRequest();
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Gets event cities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("cities")]

        [SwaggerResponse(StatusCodes.Status200OK, "Event cities", typeof(IImmutableList<EventCityModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventCitiesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<EventCityModel>>> GetEventCitiesAsync([FromQuery] int? eventTypeId)
        {
            var query = new GetEventCities(eventTypeId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return BadRequest();
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Gets events list by city and/or type id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/events")]

        [SwaggerResponse(StatusCodes.Status200OK, "Events list", typeof(IImmutableList<EventModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetEventsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<PagedResult<EventModel>>> GetEventsAsync([FromQuery]EventModelRequest modelRequest)
        {
            var command = new GetEvents(modelRequest);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }
    }
}
