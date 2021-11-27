using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DeUrgenta.User.Api.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(IdentityUser user, IList<string> roles);
    }
}