using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Admin.Api.Controller
{
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("test-localization")]
    public class LocalizationController : ControllerBase
    {
        private readonly IamI18nProvider _i18nProvider;

        public LocalizationController(IamI18nProvider i18nProvider)
        {
            _i18nProvider = i18nProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetString(string key)
        {
            var text = await _i18nProvider.Localize(key);
            return Ok(new { key, text });
        }
    }
}