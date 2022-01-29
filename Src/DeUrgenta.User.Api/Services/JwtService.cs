using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeUrgenta.Common.Auth;
using DeUrgenta.User.Api.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DeUrgenta.User.Api.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _jwtConfig;

        public JwtService(IOptions<JwtConfig> jwtConfigOptions)
        {
            _jwtConfig = jwtConfigOptions.Value;
        }

        public string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetUserClaims(user, roles)),
                Expires = DateTime.UtcNow.AddSeconds(_jwtConfig.TokenExpirationInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        private static Claim[] GetUserClaims(IdentityUser user, IList<string> roles)
        {
            var claimsBuilder = new List<Claim>
            {
                new ("Id", user.Id),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (roles == null || roles.Count == 0)
            {
                roles = new[] { ApiUserRoles.User };
            }

            foreach (var userRole in roles)
            {
                claimsBuilder.Add(new Claim(ApiUserRoles.ClaimName, userRole));
            }

            return claimsBuilder.ToArray();
        }
    }
}