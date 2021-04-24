using System;
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
using DeUrgenta.Common.Swagger;
using Microsoft.AspNetCore.Authorization;

namespace DeUrgenta.Certifications.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("certifications")]
    [Authorize]
    public class CertificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CertificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets user certifications
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [SwaggerResponse(StatusCodes.Status200OK, "User certifications", typeof(IImmutableList<CertificationModel>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetCertificationsResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<IImmutableList<CertificationModel>>> GetCertificationsAsync()
        {
            // TODO: get user id from identity
            var certifications = await _mediator.Send(new GetCertifications(Guid.NewGuid()));

            return Ok(certifications);
        }

        /// <summary>
        /// Adds a new certification
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, "New certification", typeof(CertificationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(CertificationRequest), typeof(AddOrUpdateCertificationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateCertificationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<CertificationModel>> CreateNewCertificationAsync([FromBody] CertificationRequest certification)
        {
            // TODO: get user id from identity
            var newCertificationId = await _mediator.Send(new CreateCertification(Guid.NewGuid(), certification.Name, certification.ExpirationDate));

            return Ok(newCertificationId);
        }

        /// <summary>
        /// Updates a certification
        /// </summary>
        [HttpPut]
        [Route("{certificationId:guid}")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated certification", typeof(CertificationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(CertificationRequest), typeof(AddOrUpdateCertificationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateCertificationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<CertificationModel>> UpdateCertificationAsync([FromRoute] Guid certificationId, [FromBody] CertificationRequest certification)
        {

            return NoContent();
        }

        /// <summary>
        /// Delete a certification
        /// </summary>
        [HttpDelete]
        [Route("{certificationId:guid}")]

        [SwaggerResponse(StatusCodes.Status204NoContent, "Certification was deleted")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult> DeleteCertificationAsync([FromRoute] Guid certificationId)
        {
            throw new NotImplementedException();
        }
    }
}
