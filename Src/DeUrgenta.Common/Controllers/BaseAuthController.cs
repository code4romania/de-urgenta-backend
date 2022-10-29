using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Common.Controllers
{
    public class BaseAuthController : ControllerBase
    {
        public string UserSub => User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    }
}
