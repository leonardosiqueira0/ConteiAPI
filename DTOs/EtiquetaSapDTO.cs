namespace ConteiAPI.DTOs
{
    public class EtiquetaSapDTO
    {
        public int Code { get; set; }
        public string U_CodigoProduto { get; set; }
        public string U_Lote { get; set; }
        public int U_QtdePallet { get; set; }
        public string U_QRCode { get; set; }
        public required string U_Objeto { get; set; }
        public required string U_DocEntry { get; set; }
        public required string U_Sequencial { get; set; }
    }
}
