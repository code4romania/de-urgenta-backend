using DeUrgenta.User.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace DeUrgenta.User.Api.Services
{
    public interface IJwtService
    {
        AuthResult GenerateJwtToken(IdentityUser user);
    }
}