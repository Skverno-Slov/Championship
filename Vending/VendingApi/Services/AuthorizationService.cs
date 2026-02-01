using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using VendingApi.Contexts;
using VendingApi.Models;
using WEMMWpf;

namespace WEMMApi.Services
{
    public class AuthorizationService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

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

        public string GenerateRefrashToken()
        {
            var randomNumber = new byte[32];
            using var numbers = RandomNumberGenerator.Create();
            numbers.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<RefrashToken?> GetRefrashTokenByUserIdAsync(string userId)
        {
            return await _context.RefrashTokens.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task AddRefrashTokenAsync(string userId, string stringToken)
        {
            var token = new RefrashToken()
            {
                UserId = userId,
                StringToken = BCrypt.Net.BCrypt.EnhancedHashPassword(stringToken),
                ExpiryDate = DateTime.UtcNow.AddHours(1)
            };

            await _context.RefrashTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTokenAsync(string userId)
        {
            var token = await GetRefrashTokenByUserIdAsync(userId);

            if (token != null)
            {
                _context.RefrashTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> VerifyRefreshToken(string userId, string clientToken)
        {
            var token = await GetRefrashTokenByUserIdAsync(userId);
            return token is null ? false 
                : BCrypt.Net.BCrypt.EnhancedVerify(clientToken, token.StringToken)
                && token.ExpiryDate > DateTime.UtcNow;
        }
    }
}
