using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Certifications.Api.Models;
using DeUrgenta.Certifications.Api.Queries;
using DeUrgenta.Certifications.Api.Swagger;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace DeUrgenta.Certifications.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("certifications")]
    public class CertificationCotroller : ControllerBase
    {
        private readonly IMediator _mediator;

        public CertificationCotroller(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user certifications
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCertificationsResponseExample))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        public async Task<ActionResult<IImmutableList<CertificationModel>>> GetCertificationsAsync()
        {
            // TODO: get user id from identity
            var certifications = await _mediator.Send(new GetCertifications(1));

            return Ok(certifications);
        }

        /// <summary>
        /// Adds a new certification
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(NewCertificationModel), typeof(AddNewCertificationModelExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCertificationsResponseExample))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        public async Task<ActionResult> CreateNewCertificationAsync([FromBody] NewCertificationModel request)
        {
            // TODO: get user id from identity
            var newCertificationId = await _mediator.Send(new CreateCertification(1, request.Name, request.ExpirationDate));

            return Ok(newCertificationId);
        }

        /// <summary>
        /// Updates a certification
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("{certificationId:int}")]
        [SwaggerRequestExample(typeof(NewCertificationModel), typeof(AddNewCertificationModelExample))]
        [SwaggerResponseExample(StatusCodes.Status204NoContent, typeof(void))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]
        public async Task<ActionResult> UpdateCertificationAsync([FromRoute] int certificationId, [FromBody] NewCertificationModel request)
        {

            return NoContent();
        }

        /// <summary>
        /// Delete a certification
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{certificationId:int}")]
        [SwaggerResponseExample(StatusCodes.Status204NoContent, typeof(void))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> DeleteCertificationAsync([FromRoute] int certificationId)
        {

            return NoContent();
        }
    }
}
