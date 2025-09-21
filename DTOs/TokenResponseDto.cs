using ConteiAPI.Models;

namespace ConteiAPI.DTOs
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UsuarioModel? User { get; set; }
    }
}
