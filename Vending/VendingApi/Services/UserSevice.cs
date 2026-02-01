using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VendingApi.Contexts;
using VendingApi.Models;
using WEMMApi.Dtos;

namespace WEMMApi.Services
{
    public class UserSevice(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<UserMiniProfileDto?> GetUserMiniProfileByIdAsync(string id)
        {
            if (!_context.Users.Any(u => u.UserId == id))
                return null;

            return await _context.Users.Include(u => u.Role)
                .Select(u => new UserMiniProfileDto
                {
                    UserId = u.UserId,
                    FullName = $"{u.FirstName.ElementAt(0) + "."} {(u.MiddleName == null ? string.Empty : u.MiddleName.ElementAt(0) + ".")} {u.LastName}",
                    RoleName = u.Role.Name,
                    Image = u.Image
                }).FirstOrDefaultAsync(u => u.UserId == id);
        }

        public bool IsUserExists(string email)
            => _context.Users.Any(u => u.Email == email);

    }
}
