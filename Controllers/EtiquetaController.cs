using ConteiAPI.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConteiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EtiquetaController : Controller
    {
        private readonly IEtiquetaService _etiquetaService;
        private readonly IContagemService _contagemService;

        public EtiquetaController(IEtiquetaService etiquetaService, IContagemService contagemService)
        {
            _etiquetaService = etiquetaService;
            _contagemService = contagemService;
        }

        [HttpGet]
        [Route("byQRCode/{qrcode}")]
        public IActionResult GetEtiquetaByQRCode(string qrcode, Guid contagemID)
        {
            var etiqueta = _etiquetaService.GetEtiquetaByQRCode(qrcode);

            if (etiqueta is null)
                return NotFound(new { Message = "Etiqueta não encontrada" });

            if (_contagemService.VerificarSeContagemJaFoiRealizada(contagemID, etiqueta.EtiquetaID))
                return BadRequest(new { Message = "A contagem já foi realizada para esta etiqueta." });

            return Ok(etiqueta);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEtiquetaById(int id)
        {
            var etiqueta = _etiquetaService.GetEtiquetaByID(id);

            if (etiqueta is null)
                return NotFound(new { Message = "Etiqueta não encontrada" });

            return Ok(etiqueta);
        }
    }
}
