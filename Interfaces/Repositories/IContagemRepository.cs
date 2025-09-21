using ConteiAPI.DTOs;
using ConteiAPI.Models;

namespace ConteiAPI.Interfaces.Repositories
{
    public interface IContagemRepository
    {
        void CriarContagem(ContagemModel contagemModel);
        void Finalizar(Guid id);
        void CriarContagemEtiqueta(ContagemEtiquetaModel contagemEtiquetaModel);
        bool VerificarSeContagemJaFoiRealizada(Guid contagemID, int etiquetaID);
        List<ContagemModel> ObterContagens(string? status);
        List<ContagemEtiquetaDTO> ObterContagemEtiquetas(Guid contagemID);
    }
}
