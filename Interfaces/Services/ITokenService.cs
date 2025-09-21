using ConteiAPI.Models;

namespace ConteiAPI.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid id);
        string GenerateRefreshToken();
    }
}
