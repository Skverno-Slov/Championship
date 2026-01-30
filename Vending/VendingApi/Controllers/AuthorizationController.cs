using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WEMMApi.Contexts;
using WEMMApi.Dtos;
using WEMMApi.Services;

namespace WEMMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(AppDbContext context) : ControllerBase
    {
        private readonly UserSevice _userSevice = new(context);
        private readonly AuthorizationService _authorizationService = new();

        [HttpPost("Token")]
        public async Task<ActionResult<TokenResponce>> VerifyUserAsync([FromBody] LoginRequest request)
        {
            var user = await _userSevice.GetUserByEmailAsync(request.Login);

            if (user is null)
                return BadRequest("Неверный логин или пароль");

            if (!_authorizationService.VerifyPassword(request.Password, user.HashedPassword))
                return BadRequest("Неверный логин или пароль");

            var responce = new TokenResponce();
            responce.JwtToken = _authorizationService.GenerateJwt(user);

            return responce;
        }
    }
}
