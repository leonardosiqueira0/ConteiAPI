namespace ConteiAPI.Models
{
    public class ProdutoModel
    {
        public required string Codigo { get; set; }
        public required string Descricao { get; set; }
        public required string CodigoBarras { get; set; }
        public required double QuanidadeUnidadeVenda { get; set; }
        public required string UnidadeVenda { get; set; }
        public required double QuantidadeUnidadeCompra { get; set; }
        public required double QuantidadePorItem { get; set; }
        public required string UnidadeEstoque { get; set; }
        public required int? QuantidadePallet { get; set; }
    }
}
