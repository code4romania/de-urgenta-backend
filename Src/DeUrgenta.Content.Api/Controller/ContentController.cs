using System;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Content.Api.Controller
{
    public class ContentController : ControllerBase
    {
        private readonly IamI18nProvider _i18NProvider;
        public ContentController(IamI18nProvider i18NProvider)
        {
            _i18NProvider = i18NProvider;
        }

        [HttpGet("content")]
        public async Task<IActionResult> GetAvailableContent([FromQuery] string key)
        {
            var text = await _i18NProvider.Localize(key);
            return Ok(new { key, text });
        }

        [HttpGet("content_keys")]
        public async Task<IActionResult> GetAvailableContentKeys()
        {
            var hasLanguageHeader = HttpContext.Request.Headers
            .TryGetValue("Accept-Language", out var langVal);

            if(!hasLanguageHeader) return BadRequest();

            var languageKeys = await _i18NProvider.GetAvailableContentKeys(langVal.ToString());

            return Ok(languageKeys);
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetAvailableLanguages()
        {
            var languages = await _i18NProvider.GetLanguages();
            return Ok(languages);
        }
    }
}