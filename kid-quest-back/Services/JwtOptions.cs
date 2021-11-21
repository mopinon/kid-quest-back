using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace KidQquest.Services
{
    public class JwtOptions
    {
        public const string ISSUER = "KidQquestServer"; 
        public const string AUDIENCE = "KidQquestClient";
        const string KEY = "q+t7G@P3@w!P&bE!dknR+5j-";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.Unicode.GetBytes(KEY));
        }
    }
}