using AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.BLL.Exceptions;
using UserManagementSystem.BLL.Extensions;
using UserManagementSystem.BLL.Utilities;
using UserManagementSystem.DAL.DbContexts;
using UserManagementSystem.DAL.Entities;

namespace UserManagementSystem.BLL.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManagementSystemDbContext dbContext;
        private readonly AuthorizationConfigs authorizationConfigs;

        public AuthorizationService(UserManagementSystemDbContext dbContext,
            AuthorizationConfigs authorizationConfigs)
        {
            this.dbContext = dbContext;
            this.authorizationConfigs = authorizationConfigs;
        }

        public async Task<AuthTokenModel> Login(AuthModel credentials)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(x => x.Email.ToUpper() == credentials.Login.ToUpper()) ?? throw new AppUnauthorizedException();

            if (!SecurePasswordHasher.Verify(credentials.Password, user.Password))
            {
                throw new AppUnauthorizedException();
            }

            return await GenerateToken(user);
        }

        public async Task<AuthTokenModel> Refresh(RefreshTokenModel refreshTokenModel)
        {
            var principal = GetPrincipalFromExpiredToken(refreshTokenModel.Token);
            var userId = principal.GetLoggedInUserId();

            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var oldRefreshToken = await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Token == refreshTokenModel.RefreshToken);

            if (user == null || oldRefreshToken == null)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            dbContext.RefreshTokens.Remove(oldRefreshToken);

            return await GenerateToken(user);
        }

        private async Task<AuthTokenModel> GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(authorizationConfigs.TokenKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(authorizationConfigs.TokenExpiratinInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = GenerateRefreshToken();

            await dbContext.RefreshTokens.AddAsync(new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshToken
            });

            await dbContext.SaveChangesAsync();

            return new AuthTokenModel
            {
                Token = tokenHandler.WriteToken(token),
                ExpiredIn = new DateTimeOffset(tokenDescriptor.Expires.Value).ToUnixTimeMilliseconds(),
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = TokenValidator.GetTokenValidationParameters(authorizationConfigs.TokenKey);

            //here we are saying that we don't care about the token's expiration date
            tokenValidationParameters.ValidateLifetime = false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
