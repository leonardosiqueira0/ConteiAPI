using Microsoft.AspNetCore.Identity.Data;
using ConteiAPI.Models;

namespace ConteiAPI.Interfaces
{
    public interface ILoginRepository
    {
        UsuarioModel? Login(DTOs.LoginRequestDto loginRequest);
    }
}
