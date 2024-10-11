namespace FurniFusion.Dtos.SuperAdmin.InventoryProduct
{
    public class InventoryProductDto
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public int? Quantity { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
