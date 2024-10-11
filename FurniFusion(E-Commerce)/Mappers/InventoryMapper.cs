using FurniFusion.Dtos.SuperAdmin.Inventory;
using FurniFusion.Models;

namespace FurniFusion.Mappers
{
    public static class InventoryMapper
    {
        public static InventoryDto ToInventoryDto(this Inventory inventory)
        {
            return new InventoryDto
            {
                InventoryId = inventory.InventoryId,
                WarehouseLocation = inventory.WarehouseLocation,
                IsActive = inventory.IsActive,
                CreatedAt = inventory.CreatedAt,
                UpdatedAt = inventory.UpdatedAt,
                InventoryProducts = inventory.InventoryProducts.Select(IP => IP.ToInventoryProductDto()).ToList()

            };
        }
    }
}
