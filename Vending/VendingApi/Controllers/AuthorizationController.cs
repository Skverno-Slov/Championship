using Microsoft.AspNetCore.Mvc;
using VendingApi.Contexts;
using VendingApi.Dtos;
using VendingApi.Models;
using WEMMApi.Dtos;
using WEMMApi.Services;

namespace WEMMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController(AppDbContext context) : ControllerBase
    {
        private readonly UserSevice _userSevice = new(context);
        private readonly AuthorizationService _authorizationService = new(context);

        [HttpPost("Login")]
        public async Task<ActionResult<TokenResponce>> VerifyUserAsync([FromBody] LoginRequest request)
        {
            var user = await _userSevice.GetUserByEmailAsync(request.Login);

            if (user is null)
                return BadRequest("Неверный логин или пароль");

            if (!_authorizationService.VerifyPassword(request.Password, user.HashedPassword))
                return BadRequest("Неверный логин или пароль");

            return Createresponce(user);
        }

        [HttpPost("Refrash")]
        public async Task<ActionResult<TokenResponce>> VerifyTokenAsync([FromBody] RefrashRequest request)
        {
            var user = await _userSevice.GetUserByIdAsync(request.UserId);

            if (user is null)
                return BadRequest("неверный id");

            if (!await _authorizationService.VerifyRefreshToken(request.UserId, request.StringToken))
                return BadRequest("Неверный токен");

            var newRefrashToken = _authorizationService.GenerateRefrashToken();

            var responce = new TokenResponce()
            {
                JwtToken = _authorizationService.GenerateJwt(user),
                RefreshToken = newRefrashToken
            };

            await _authorizationService.RemoveTokenAsync(user.UserId);
            await _authorizationService.AddRefrashTokenAsync(user.UserId, newRefrashToken);

            return responce;
        }

        private TokenResponce Createresponce(User user)
        {
            var responce = new TokenResponce();
            responce.JwtToken = _authorizationService.GenerateJwt(user);
            responce.RefreshToken = _authorizationService.GenerateRefrashToken();
            return responce;
        }
    }
}
