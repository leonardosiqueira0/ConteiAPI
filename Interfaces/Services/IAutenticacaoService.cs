using ConteiAPI.Models;

namespace ConteiAPI.Interfaces
{
    public interface IAutenticacaoService
    {
        void CreateRefreshToken(RefreshTokenModel refreshToken);
        RefreshTokenModel? GetRefreshToken(string token);

        void RevokeRefreshToken(string token);
    }
}
