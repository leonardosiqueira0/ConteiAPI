using ConteiAPI.DTOs;
using ConteiAPI.Interfaces;
using ConteiAPI.Models;

namespace ConteiAPI.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public UsuarioModel? Login(LoginRequestDto loginRequest)
        {
            return _loginRepository.Login(loginRequest);
        }
    }
}
