using Microsoft.AspNetCore.Identity.Data;
using ConteiAPI.Models;

namespace ConteiAPI.Interfaces
{
    public interface ILoginService
    {
        UsuarioModel? Login(DTOs.LoginRequestDto loginRequest);
    }
}
