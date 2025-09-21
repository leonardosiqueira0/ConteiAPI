using ConteiAPI.Models;

namespace ConteiAPI.Interfaces.Repositories
{
    public interface IAutenticacaoRepository
    {
        void CreateRefreshToken(RefreshTokenModel refreshToken);
        RefreshTokenModel? GetRefreshToken(string token);

        void RevokeRefreshToken(string token);
    }
}
