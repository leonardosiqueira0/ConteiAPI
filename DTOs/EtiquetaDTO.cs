using ConteiAPI.Models;

namespace ConteiAPI.DTOs
{
    public class EtiquetaDTO
    {
        public int EtiquetaID { get; set; }
        public required ProdutoModel Produto { get; set; }
        public string? Lote { get; set; }
        public int QuantidadePallet { get; set; }
        public required string QRCode { get; set; }
        public required string TipoObjeto { get; set; }
        public required int NumeroDocumento { get; set; }
        public required int SequencialEtiqueta { get; set; }
    }
}
