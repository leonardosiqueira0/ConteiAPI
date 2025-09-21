using ConteiAPI.DTOs;
using ConteiAPI.Helpers;
using ConteiAPI.Interfaces;
using ConteiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConteiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;
        private readonly Interfaces.Repositories.IAutenticacaoRepository _authRepository;
        private readonly ILoginService _loginService;

        public LoginController(ITokenService tokenService, Interfaces.Repositories.IAutenticacaoRepository authRepository, IOptions<JwtSettings> jwtSettings, ILoginService loginService)
        {
            _tokenService = tokenService;
            _authRepository = authRepository;
            _jwtSettings = jwtSettings.Value;
            _loginService = loginService;
        }

        [HttpPost()]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            UsuarioModel? user = _loginService.Login(request);

            if (user == null)
            {
                return BadRequest("Usuário ou senha inválidos");
            }

            if (user.SapID == null)
            {
                return BadRequest("Usuário não possui ID do SAP");
            }

            if (user.Label == null)
            {
                return BadRequest("Usuário não possui nível de acesso definido");
            }

            var accessToken = _tokenService.GenerateAccessToken(user.Id);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshModel = new RefreshTokenModel
            {
                Token = refreshToken,
                ExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                IsRevoked = false,
                UserId = user.Id,
            };

            _authRepository.CreateRefreshToken(refreshModel);


            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                User = user
            });
        }
    }
}
