using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UserManagementSystem.BLL.Utilities
{
    public class TokenValidator
    {
        public static TokenValidationParameters GetTokenValidationParameters(string tokenKey)
        {
            var key = Encoding.ASCII.GetBytes(tokenKey);

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };
        }
    }
}
