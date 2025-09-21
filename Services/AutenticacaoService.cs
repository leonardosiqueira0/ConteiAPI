using ConteiAPI.DTOs;
using ConteiAPI.Interfaces;
using ConteiAPI.Interfaces.Repositories;
using ConteiAPI.Models;

namespace ConteiAPI.Services
{
    public class AutenticacaoService : Interfaces.IAutenticacaoService
    {
        private readonly Interfaces.Repositories.IAutenticacaoRepository _authRepository;

        public AutenticacaoService (Interfaces.Repositories.IAutenticacaoRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public void CreateRefreshToken(RefreshTokenModel refreshToken)
        {
            _authRepository.CreateRefreshToken(refreshToken);
        }

        public RefreshTokenModel? GetRefreshToken(string token)
        {
            return _authRepository.GetRefreshToken(token);
        }

        public UsuarioModel? Login(LoginRequestDto loginRequest)
        {
            throw new NotImplementedException();
        }

        public void RevokeRefreshToken(string token)
        {
            _authRepository.RevokeRefreshToken(token);
        }
    }
}
