using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.Admin.Api.Swagger.AdminLocalization;
using DeUrgenta.Common.Auth;
using DeUrgenta.Common.Swagger;
using DeUrgenta.I18n.Service.Models;
using DeUrgenta.I18n.Service.Providers;
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
        private readonly IamI18nProvider _i18NProvider;

        public AdminLocalizationController(IamI18nProvider i18NProvider)
        {
            _i18NProvider = i18NProvider;
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
        public async Task<IActionResult> GetString(string key)
        {
            var text = await _i18NProvider.Localize(key);
            return Ok(new StringResourceModel { Key = key, Value = text });
        }

        /// <summary>
        /// Add new resource
        /// </summary>
        [SwaggerResponse(StatusCodes.Status200OK, "Added/Updated Content for specific key/culture combination", typeof(StringResourceModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "A business rule was violated", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Something bad happened", typeof(ProblemDetails))]

        [SwaggerResponseExample(StatusCodes.Status200OK,
        typeof(AddOrUpdateContentResponseExample))]
        [SwaggerRequestExample(typeof(AddOrUpdateContentModel),
        typeof(AddOrUpdateContentRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BusinessRuleViolationResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status500InternalServerError, typeof(ApplicationErrorResponseExample))]
        [HttpPost]
        public async Task<ActionResult<StringResourceModel>> AddOrUpdateContent(AddOrUpdateContentModel contentModel)
        {
            var updatedContent = await _i18NProvider.AddOrUpdateContentValue(contentModel.Culture,
            contentModel.Key, contentModel.Value);

            if (updatedContent == null) return BadRequest("Specified culture does not exist");

            return Ok(updatedContent);
        }
    }
}