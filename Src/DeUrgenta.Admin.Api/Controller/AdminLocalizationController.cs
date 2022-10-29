using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Commands;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Swagger.AdminLocalization;
using DeUrgenta.Common.Mappers;
using DeUrgenta.Common.Auth;
using DeUrgenta.Common.Swagger;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.I18n.Service.Providers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Admin.Api.Controller
{
    [Authorize(Policy = ApiPolicies.AdminOnly)]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("admin/content")]
    public class AdminLocalizationController : ControllerBase
    {
        private readonly IamI18nProvider _i18nProvider;
        private readonly IMediator _mediator;
        private readonly IResultMapper _mapper;

        public AdminLocalizationController(IamI18nProvider i18NProvider, IMediator mediator, IResultMapper mapper)
        {
            _i18nProvider = i18NProvider;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get available content for specific key
        /// </summary>
        [SwaggerResponse(StatusCodes.Status200OK, "Available Content for key", typeof(StringResourceModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK,
        typeof(GetStringResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        [HttpGet]
        public async Task<IActionResult> GetString(string key, CancellationToken ct)
        {
            var text = await _i18nProvider.Localize(key, ct);
            return Ok(new StringResourceModel
            {
                Key = key,
                Value = text
            });
        }

        /// <summary>
        /// Add new resource
        /// </summary>
        [SwaggerResponse(StatusCodes.Status200OK, "Added/Updated Content for specific key/culture combination", typeof(StringResourceModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddOrUpdateContentResponseExample))]
        [SwaggerRequestExample(typeof(AddOrUpdateContentModel), typeof(AddOrUpdateContentRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        [HttpPost]
        public async Task<ActionResult<StringResourceModel>> AddOrUpdateContent(AddOrUpdateContentModel model, CancellationToken ct)
        {
            var result = await _mediator.Send(new AddOrUpdateContent(model.Culture, model.Key, model.Value), ct);

            return await _mapper.MapToActionResult(result, ct);
        }
    }
}