namespace ConteiAPI.Models
{
    public class EtiquetaModel
    {
        public int EtiquetaID { get; set; }
        public required string ProdutoID { get; set; }
        public string? Lote { get; set; }
        public int QuantidadePallet { get; set; }
        public required string QRCode { get; set; }
        public required string TipoObjeto { get; set; }
        public required int NumeroDocumento { get; set; }
        public required int SequencialEtiqueta { get; set; }
    }
}
