using FurniFusion.Dtos.SuperAdmin.Inventory;
using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface IInventoryService
    {
        Task<ServiceResult<List<Inventory>>> GetAllInventoriesAsync();
        Task<ServiceResult<Inventory>> GetInventoryByIdAsync(int? id);
        Task<ServiceResult<Inventory>> CreateInventoryAsync(CreateInventoryDto inventoryDto);
        Task<ServiceResult<Inventory>> UpdateInventoryAsync(UpdateInventoryDto inventoryDto);
        Task<ServiceResult<bool>> DeleteInventoryAsync(int id);
    }
}
