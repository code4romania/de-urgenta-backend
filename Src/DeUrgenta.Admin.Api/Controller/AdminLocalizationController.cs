using System.Threading.Tasks;
using DeUrgenta.Admin.Api.Models;
using DeUrgenta.I18n.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Admin.Api.Controller
{
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

        [HttpGet]
        public async Task<IActionResult> GetString(string key)
        {
            var text = await _i18NProvider.Localize(key);
            return Ok(new { key, text });
        }


        [HttpPost]
        public async Task<IActionResult> AddOrUpdateContent(AddOrUpdateContentModel contentModel)
        {
            var updatedContent = await _i18NProvider.AddOrUpdateContentValue(contentModel.Culture,
            contentModel.Key, contentModel.Value);

            if(updatedContent== null) return BadRequest("Specified culture does not exist");

            return Ok(updatedContent);
        }
    }
}