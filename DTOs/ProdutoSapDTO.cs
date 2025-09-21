namespace ConteiAPI.DTOs
{
    public class ProdutoSapDTO
    {
        public required string ItemCode { get; set; }
        public required string ItemName { get; set; }
        public required string BarCode { get; set; }
        public required double SalesItemsPerUnit { get; set; }
        public required double PurchaseItemsPerUnit { get; set; }
        public required double CountingItemsPerUnit { get; set; }
        public required string InventoryUOM { get; set; }
        public required string SalesPackagingUnit { get; set; }
        public required int? U_LGO_QuantPallet { get; set; }
    }
}
