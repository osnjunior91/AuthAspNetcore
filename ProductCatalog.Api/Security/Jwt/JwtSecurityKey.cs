using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProductCatalog.Api.Security.Jwt
{
    public class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret) =>
                 new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
    }
}
