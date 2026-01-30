using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VendingApi.Contexts;
using WEMMApi.Dtos;
using WEMMApi.Services;

namespace WEMMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(AppDbContext context) : ControllerBase
    {
        private readonly UserSevice _userSevice = new(context);

        [HttpGet("MiniProfile")]
        [Authorize]
        public async Task<ActionResult<UserMiniProfileDto>> GetUserMiniProfileAsync()
        {
            var userProfile = await _userSevice
                .GetUserMiniProfileByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (userProfile is null)
            {
                return BadRequest("Пользователь не найден");
            }

            return userProfile;
        }
    }
}
