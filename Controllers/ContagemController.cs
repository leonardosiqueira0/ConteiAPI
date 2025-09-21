using ConteiAPI.DTOs;
using ConteiAPI.Interfaces.Services;
using ConteiAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConteiAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContagemController : Controller
    {
        private readonly IContagemService _contagemService;

        public ContagemController(IContagemService contagemService)
        {
            _contagemService = contagemService;
        }

        [HttpPost("criar")]
        public IActionResult CriarContagem([FromBody] ContagemModel contagemModel)
        {
            _contagemService.CriarContagem(contagemModel);
            return Ok();
        }

        [HttpPost("criar-etiqueta")]
        public IActionResult CriarContagemEtiqueta([FromBody] ContagemEtiquetaModel contagemEtiquetaModel)
        {
            try
            {
                _contagemService.CriarContagemEtiqueta(contagemEtiquetaModel);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<List<ContagemModel>> GetContagens(string? status)
        {
            var contagens = _contagemService.ObterContagens(status);
            return Ok(contagens);
        }

        [HttpGet("{id}/finalizar")]
        public IActionResult Finalizar(Guid id)
        {
            _contagemService.Finalizar(id);
            return Ok();
        }

        [HttpGet("{contagemID}/etiquetas")]
        public ActionResult<List<ContagemEtiquetaDTO>> GetContagemEtiquetas(Guid contagemID)
        {
            var etiquetas = _contagemService.ObterContagemEtiquetas(contagemID);
            return Ok(etiquetas);
        }
    }
}
