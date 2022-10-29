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
using DeUrgenta.Common.Controllers;
using DeUrgenta.Common.Mappers;

namespace DeUrgenta.Certifications.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("certifications")]
    [Authorize]
    public class CertificationController : BaseAuthController
    {
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public CertificationController(IMediator mediator, IResultMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
            var query = new GetCertifications(UserSub);
            var result = await _mediator.Send(query);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Adds a new certification
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]

        [SwaggerResponse(StatusCodes.Status200OK, "New certification", typeof(CertificationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(CertificationRequest), typeof(AddOrUpdateCertificationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateCertificationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<CertificationModel>> CreateNewCertificationAsync([FromForm] CertificationRequest certification)
        {
            var command = new CreateCertification(UserSub, certification);
            var result = await _mediator.Send(command);

            return await _mapper.MapToActionResult(result);
        }

        /// <summary>
        /// Updates a certification
        /// </summary>
        [HttpPut]
        [Route("{certificationId:guid}")]
        [Consumes("multipart/form-data")]

        [SwaggerResponse(StatusCodes.Status200OK, "Updated certification", typeof(CertificationModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerRequestExample(typeof(CertificationRequest), typeof(AddOrUpdateCertificationRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateCertificationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        public async Task<ActionResult<CertificationModel>> UpdateCertificationAsync([FromRoute] Guid certificationId, [FromForm] CertificationRequest certification)
        {
            var command = new UpdateCertification(UserSub, certificationId, certification);
            var result = await _mediator.Send(command);

            return await _mapper.MapToActionResult(result);
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
        public async Task<IActionResult> DeleteCertificationAsync([FromRoute] Guid certificationId)
        {
            var command = new DeleteCertification(UserSub, certificationId);
            var result = await _mediator.Send(command);

            return await _mapper.MapToActionResult(result);
        }
    }
}
