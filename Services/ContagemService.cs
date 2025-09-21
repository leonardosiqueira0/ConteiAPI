using ConteiAPI.DTOs;
using ConteiAPI.Interfaces.Services;
using ConteiAPI.Models;
using ConteiAPI.Repositories;
using System;
using System.Collections.Generic;

namespace ConteiAPI.Services
{
    public class ContagemService(IConfiguration configuration, IEtiquetaService etiquetaService) : BasicoRepository(configuration), IContagemService
    {
        private readonly ContagemRepository _contagemRepository = new(configuration, etiquetaService);

        public void CriarContagem(ContagemModel contagemModel)
        {
            _contagemRepository.CriarContagem(contagemModel);
        }

        public void CriarContagemEtiqueta(ContagemEtiquetaModel contagemEtiquetaModel)
        {
            if (VerificarSeContagemJaFoiRealizada(contagemEtiquetaModel.ContagemID, contagemEtiquetaModel.EtiquetaID))
                throw new InvalidOperationException("Esta etiqueta já foi contada para esta contagem.");

            _contagemRepository.CriarContagemEtiqueta(contagemEtiquetaModel);
        }

        public bool VerificarSeContagemJaFoiRealizada(Guid contagemID, int etiquetaID)
        {
            return _contagemRepository.VerificarSeContagemJaFoiRealizada(contagemID, etiquetaID);
        }

        public List<ContagemModel> ObterContagens(string? status)
        {
            return _contagemRepository.ObterContagens(status);
        }

        public List<ContagemEtiquetaDTO> ObterContagemEtiquetas(Guid contagemID)
        {
            return _contagemRepository.ObterContagemEtiquetas(contagemID);
        }

        public void Finalizar(Guid id)
        {
            _contagemRepository.Finalizar(id);
        }
    }
}
