using FurniFusion.Data;
using FurniFusion.Dtos.SuperAdmin.Inventory;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly FurniFusionDbContext _context;

        public InventoryService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Inventory>>> GetAllInventoriesAsync()
        {

            var inventories =  await _context.Inventories
                .Include(I => I.InventoryProducts)
                .ToListAsync();

            return ServiceResult<List<Inventory>>.SuccessResult(inventories);
        }

        public async Task<ServiceResult<Inventory>> GetInventoryByIdAsync(int? id)
        {
            var inventory = await _context.Inventories
                .Include(I => I.InventoryProducts)
                .FirstOrDefaultAsync(I => I.InventoryId == id);

            if (inventory == null)
            {
                return ServiceResult<Inventory>.ErrorResult("Inventory not found", StatusCodes.Status404NotFound);
            }

            return ServiceResult<Inventory>.SuccessResult(inventory);
        }

        public async Task<ServiceResult<Inventory>> CreateInventoryAsync(CreateInventoryDto inventoryDto)
        {
            var result = await _context.Inventories.FirstOrDefaultAsync(I => I.WarehouseLocation == inventoryDto.WarehouseLocation);

            if (result != null)
            {
                return ServiceResult<Inventory>.ErrorResult("Inventory already exists", StatusCodes.Status409Conflict);
            }

            var newInventory = new Inventory
            {
                WarehouseLocation = inventoryDto.WarehouseLocation,
                IsActive = inventoryDto.IsActive,
                CreatedAt = DateTime.Now,               
            };

            await _context.Inventories.AddAsync(newInventory);
            await _context.SaveChangesAsync();

            return ServiceResult<Inventory>.SuccessResult(newInventory, "Inventory created successfully", StatusCodes.Status201Created);
        }

        public async Task<ServiceResult<Inventory>> UpdateInventoryAsync(UpdateInventoryDto inventoryDto)
        {
            var result = await GetInventoryByIdAsync(inventoryDto.InventoryId);

            if (result == null || result.Data == null)
            {
                return ServiceResult<Inventory>.ErrorResult("Inventory not found", StatusCodes.Status404NotFound);
            }

            var existingInventory = await _context.Inventories.FirstOrDefaultAsync(I => I.WarehouseLocation == inventoryDto.WarehouseLocation);

            if (existingInventory != null && existingInventory.InventoryId != inventoryDto.InventoryId)
            {
                return ServiceResult<Inventory>.ErrorResult("Inventory already exists", StatusCodes.Status409Conflict);
            }

            result.Data.WarehouseLocation = inventoryDto.WarehouseLocation ?? result.Data.WarehouseLocation;
            result.Data.IsActive = inventoryDto.IsActive ?? result.Data.IsActive;
            result.Data.UpdatedAt = DateTime.Now;

            _context.Inventories.Update(result.Data);
            await _context.SaveChangesAsync();
            
            return ServiceResult<Inventory>.SuccessResult(result.Data, "Inventory updated successfully");
        }

        public async Task<ServiceResult<bool>> DeleteInventoryAsync(int id)
        {
            var result = await GetInventoryByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Inventory not found", StatusCodes.Status404NotFound);
            }

            _context.Inventories.Remove(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Inventory deleted successfully");
        }
    }
}
