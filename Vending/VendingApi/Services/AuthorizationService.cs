using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VendingApi.Models;
using WEMMWpf;

namespace WEMMApi.Services
{
    public class AuthorizationService()
    {
        public bool VerifyPassword(string requestPassword, string userPassword)
            => BCrypt.Net.BCrypt.EnhancedVerify(requestPassword, userPassword);

        public string GetHashedPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password, 11);

        public string GenerateJwt(User user)
        {
            var jwtLifeTime = DateTime.UtcNow.AddMinutes(15);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.issuer,
                audience: AuthOptions.audience,
                claims: claims,
                signingCredentials: credentials,
                expires: jwtLifeTime
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
