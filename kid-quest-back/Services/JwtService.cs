using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace KidQquest.Services
{
    public class JwtService : IJwtService
    {
        public string Encode(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email)
            };
            var identity = new ClaimsIdentity(claims, "Token", "Name", ClaimsIdentity.DefaultRoleClaimType);
            
            var jwt = new JwtSecurityToken(
                JwtOptions.ISSUER,
                JwtOptions.AUDIENCE,
                identity.Claims,
                null,
                null,
                new SigningCredentials(JwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public string Decode(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwt);
            var identity = new ClaimsIdentity(jwtSecurityToken.Claims);
            return identity.Name;
        }
        
        public bool IsJwtToken(string jwt)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwt);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}