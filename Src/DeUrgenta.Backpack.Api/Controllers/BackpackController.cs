using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeUrgenta.Backpack.Api.Controllers
{
    [ApiController]
    [Route("backpack")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(AuthenticationSchemes = "backpackApiAuthenticationScheme")]
    public class BackpackController : ControllerBase
    {
        /// <summary>
        /// Method for returning bla bla bla
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResponseAsync()
        {
            await Task.FromResult(0);
            return Ok("blalalalal");
        }
    }
}
