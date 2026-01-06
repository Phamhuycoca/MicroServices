using AuthService.Application.DTO;
using AuthService.Application.IServices;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.API.Controllers
{
    [ApiController]
    [Route("api/auth-token")]
    public class AuthTokenController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceRefreshToken _serviceRefreshToken;
        private readonly UserManager<nguoi_dung> _userManager;

        public AuthTokenController(IMemoryCache cache, IServiceRefreshToken serviceRefreshToken, UserManager<nguoi_dung> userManager)
        {
            _cache = cache;
            _serviceRefreshToken= serviceRefreshToken;
            _userManager = userManager;
        }

        [HttpGet("one-time-token")]
        public async Task<IActionResult> GetTokenByCode([FromQuery] string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new { message = "Code không được để trống" });

            if (!_cache.TryGetValue(code, out object cachedValue))
                return Unauthorized(new { message = "Code hết hạn hoặc không hợp lệ" });

            TokenResponseDto token;

            if (cachedValue is TokenResponseDto t)
            {
                token = t;
            }
            else if (cachedValue is Task<TokenResponseDto> task)
            {
                token = await task;
            }
            else
            {
                return Unauthorized(new { message = "Token không hợp lệ" });
            }

            _cache.Remove(code); // chống replay

            return Ok(token);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Unauthorized(new { message = "Refresh token không hợp lệ" });

            var storedToken = await _serviceRefreshToken.GetByTokenAsync(request);
            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized(new { message = "Refresh token hết hạn" });

            var user = await _userManager.FindByIdAsync(storedToken.nguoi_dung_id.ToString());
            if (user == null)
                return Unauthorized();

            // 🔥 Rotate token (rất nên)
            storedToken.IsRevoked = true;
            await _serviceRefreshToken.UpdateToken(storedToken);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var newTokens = await _serviceRefreshToken.GenerateTokensAsync(user, ip);

            return Ok(newTokens); 
        }

    }
}
