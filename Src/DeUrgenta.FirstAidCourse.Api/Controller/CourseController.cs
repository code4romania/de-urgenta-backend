using System;
using DeUrgenta.Courses.Api.Commands;
using DeUrgenta.Courses.Api.Models;
using DeUrgenta.Courses.Api.Queries;
using DeUrgenta.Courses.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Common.Swagger;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace DeUrgenta.Courses.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("courses")]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets course types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("types")]

        [SwaggerResponse(StatusCodes.Status200OK, "Course types", typeof(IImmutableList<CourseTypeModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCourseTypesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<CourseTypeModel>>> GetCourseTypesAsync()
        {
            var query = new GetCourseTypes();
            var result = await _mediator.Send(query);
            
            if (result.IsFailure)
            {
                return BadRequest();
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Gets course cities
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("cities")]

        [SwaggerResponse(StatusCodes.Status200OK, "Course cities", typeof(IImmutableList<CourseCityModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCourseCitiesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<CourseCityModel>>> GetCourseCitiesAsync()
        {
            var query = new GetCourseCities();
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return BadRequest();
            }
            return Ok(result.Value);
        }

    }
}
