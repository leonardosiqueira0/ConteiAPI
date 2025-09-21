using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ConteiAPI.DTOs;
using ConteiAPI.Helpers;
using ConteiAPI.Interfaces;
using ConteiAPI.Models;

namespace ConteiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IAutenticacaoService _authService;
        private readonly JwtSettings _jwtSettings;

        public AutenticacaoController(ITokenService tokenService, IAutenticacaoService authService, IOptions<JwtSettings> jwtSettings)
        {
            _tokenService = tokenService;
            _authService = authService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("Refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var existingToken = _authService.GetRefreshToken(request.RefreshToken);
            
            // Validar se o Token é válido
            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpirationDate <= DateTime.UtcNow)
            {
                return Unauthorized("Token inválido ou expirado");
            }

            // Gera um novo access token
            var newAccessToken = _tokenService.GenerateAccessToken(existingToken.UserId);


            // Gera e salva um novo token
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refreshModel = new RefreshTokenModel
            {
                Token = newRefreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                IsRevoked = false,
                UserId = existingToken.UserId
            };

            _authService.CreateRefreshToken(refreshModel);
            _authService.RevokeRefreshToken(existingToken.Token);

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            });
        }
    }
}
