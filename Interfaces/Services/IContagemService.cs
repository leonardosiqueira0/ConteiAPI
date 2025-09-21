using ConteiAPI.DTOs;
using ConteiAPI.Models;
using System;
using System.Collections.Generic;

namespace ConteiAPI.Interfaces.Services
{
    public interface IContagemService
    {
        void CriarContagem(ContagemModel contagemModel);
        void CriarContagemEtiqueta(ContagemEtiquetaModel contagemEtiquetaModel);
        void Finalizar(Guid id);
        bool VerificarSeContagemJaFoiRealizada(Guid contagemID, int etiquetaID);
        List<ContagemModel> ObterContagens(string? status);
        List<ContagemEtiquetaDTO> ObterContagemEtiquetas(Guid contagemID);
    }
}
