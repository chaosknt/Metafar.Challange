using Metafar.Challange.Common.Helpers;
using Metafar.Challange.Data.Service.Managers.Models;
using Metafar.Challange.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Metafar.Challange.JwtHelpers
{
    public static class JwtHelpers
    {
        public static IEnumerable<Claim> GetClaims(this User userAccounts)
            => ClaimHelper.BuildDefaultClaims(userAccounts.Id.ToString(), userAccounts.Name, userAccounts.AccountNumber);

        public static JWTResult GenTokenkey(User model, JwtSettings jwtSettings)
        {
            if (model == null) throw new ArgumentException(nameof(model));
            return BuildToken(GetClaims(model), jwtSettings);
        }

        public static JWTResult GenTokenkey(IEnumerable<Claim> claims, JwtSettings jwtSettings)
        {
            if (claims == null || !claims.Any()) throw new ArgumentException(nameof(claims));
            return BuildToken(claims, jwtSettings);
        }

        private static JWTResult BuildToken(IEnumerable<Claim> claims, JwtSettings jwtSettings)
        {
            var UserToken = new JWTResult();
            var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
            DateTime expireTime = DateTime.UtcNow.AddDays(1);
            var JWToken = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: claims,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(expireTime).DateTime,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256));
            UserToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            UserToken.RefreshToken = GenerateRefreshToken();
            UserToken.ExpirationTime = DateTime.Now.AddDays(jwtSettings.RefreshTokenValidityInDays);
            return UserToken;
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken.GetType().Name != typeof(JwtSecurityToken).Name)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var castedSecurityToken = securityToken as JwtSecurityToken;


            if (!castedSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;

        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
