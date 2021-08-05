using System;
using DeUrgenta.FirstAidCourse.Api.Commands;
using DeUrgenta.FirstAidCourse.Api.Models;
using DeUrgenta.FirstAidCourse.Api.Queries;
using DeUrgenta.FirstAidCourse.Api.Swagger;
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

namespace DeUrgenta.FirstAidCourse.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("certifications")]
    [Authorize]
    public class FirstAidCourseController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FirstAidCourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user certifications
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [SwaggerResponse(StatusCodes.Status200OK, "User certifications", typeof(IImmutableList<FirstAidCourseModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetFirstAidCoursesResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<FirstAidCourseModel>>> GetCertificationsAsync()
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var query = new GetFirstAidCourses(sub);
            var result = await _mediator.Send(query);
            
            if (result.IsFailure)
            {
                return BadRequest();
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Adds a new certification
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "New certification", typeof(FirstAidCourseModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(FirstAidCourseRequest), typeof(AddOrUpdateFirstAidCourseRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateFirstAidCourseResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<FirstAidCourseModel>> CreateNewCertificationAsync([FromBody] FirstAidCourseRequest firstAidCourse)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new CreateFirstAidCourse(sub, firstAidCourse);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Updates a certification
        /// </summary>
        [HttpPut]
        [Route("{certificationId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated certification", typeof(FirstAidCourseModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(FirstAidCourseRequest), typeof(AddOrUpdateFirstAidCourseRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateFirstAidCourseResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<FirstAidCourseModel>> UpdateFirstAidCourseAsync([FromRoute] Guid firstAidCourseId, [FromBody] FirstAidCourseRequest firstAidCourse)
        {

            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var command = new UpdateFirstAidCourse(sub, firstAidCourseId, firstAidCourse);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a FirstAidCourse
        /// </summary>
        [HttpDelete]
        [Route("{firstAidCourseId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "FirstAidCourse was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> DeleteFirstAidCourseAsync([FromRoute] Guid firstAidCourse)
        {
            var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            var command = new DeleteFirstAidCourse(sub, firstAidCourse);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
