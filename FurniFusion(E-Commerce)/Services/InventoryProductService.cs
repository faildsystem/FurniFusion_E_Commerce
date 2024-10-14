//using FurniFusion.Data;
//using FurniFusion.Dtos.SuperAdmin.InventoryProduct;
//using FurniFusion.Interfaces;
//using FurniFusion.Models;
//using Microsoft.EntityFrameworkCore;

//namespace FurniFusion.Services
//{
//    public class InventoryProductService : IInventoryProductService
//    {
//        private readonly FurniFusionDbContext _context;

//        public InventoryProductService(FurniFusionDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ServiceResult<List<InventoryProduct>>> GetAllInventoryProductsAsync(int inventoryId)
//        {
//            var inventory = await _context.Inventories.FirstOrDefaultAsync(I => I.InventoryId == inventoryId);

//            if (inventory == null)
//            {
//                return ServiceResult<List<InventoryProduct>>.ErrorResult("Inventory not found", StatusCodes.Status404NotFound);
//            }

//            var inventoryProducts = await _context.InventoryProducts.Where(IP => IP.InventoryId == inventoryId).ToListAsync();

//            return ServiceResult<List<InventoryProduct>>.SuccessResult(inventoryProducts);
//        }

//        public async Task<ServiceResult<InventoryProduct>> CreateInventoryProductAsync(CreateInventoryProductDto inventoryProductDto)
//        {

//            var inventory = await _context.Inventories.FirstOrDefaultAsync(I => I.InventoryId == inventoryProductDto.InventoryId);

//            if (inventory == null)
//            {
//                return ServiceResult<InventoryProduct>.ErrorResult("Inventory not found", StatusCodes.Status404NotFound);
//            }

//            var product = await _context.Products.FirstOrDefaultAsync(P => P.ProductId == inventoryProductDto.ProductId);

//            if (product == null)
//            {
//                return ServiceResult<InventoryProduct>.ErrorResult("Product not found", StatusCodes.Status404NotFound);
//            }

//            var result = await _context.InventoryProducts.FirstOrDefaultAsync(IP => IP.InventoryId == inventoryProductDto.InventoryId && IP.ProductId == inventoryProductDto.ProductId);

//            if (result != null)
//            {
//                return ServiceResult<InventoryProduct>.ErrorResult("Product already exists in this inventory", StatusCodes.Status409Conflict);
//            }

//            var newInventoryProduct = new InventoryProduct
//            {
//                InventoryId = inventoryProductDto.InventoryId,
//                ProductId = inventoryProductDto.ProductId,
//                Quantity = inventoryProductDto.Quantity,
//                CreatedAt = DateTime.Now,
//                UpdatedAt = DateTime.Now  
//            };

//            await _context.InventoryProducts.AddAsync(newInventoryProduct);

//            product.StockQuantity += inventoryProductDto.Quantity;
//            product.IsAvailable = product.StockQuantity > 0 ? true : false;
//            product.UpdatedAt = DateTime.Now;

//            _context.Products.Update(product);

//            await _context.SaveChangesAsync();

//            return ServiceResult<InventoryProduct>.SuccessResult(newInventoryProduct, "Product Added successfully", StatusCodes.Status201Created);
//        }

//        public async Task<ServiceResult<InventoryProduct>> UpdateInventoryProductAsync(UpdateInventoryProductDto inventoryProductDto)
//        {
//            var inventory = await _context.Inventories.FirstOrDefaultAsync(I => I.InventoryId == inventoryProductDto.InventoryId);

//            if (inventory == null)
//            {
//                return ServiceResult<InventoryProduct>.ErrorResult("Inventory not found", StatusCodes.Status404NotFound);
//            }

//            var product = await _context.Products.FirstOrDefaultAsync(P => P.ProductId == inventoryProductDto.ProductId);

//            if (product == null)
//            {
//                return ServiceResult<InventoryProduct>.ErrorResult("Product not found", StatusCodes.Status404NotFound);
//            }

//            var inventoryProduct = await _context.InventoryProducts.FirstOrDefaultAsync(IP => IP.InventoryId == inventoryProductDto.InventoryId && IP.ProductId == inventoryProductDto.ProductId);

//            if (inventoryProduct == null)
//            {
//                return ServiceResult<InventoryProduct>.ErrorResult("Product not found in this inventory", StatusCodes.Status404NotFound);
//            }

//            product.StockQuantity += inventoryProductDto.Quantity - inventoryProduct.Quantity;
//            product.IsAvailable = product.StockQuantity > 0 ? true : false;
//            product.UpdatedAt = DateTime.Now;

//            inventoryProduct.Quantity = inventoryProductDto.Quantity;
//            inventoryProduct.UpdatedAt = DateTime.Now;

//            _context.InventoryProducts.Update(inventoryProduct);
//            _context.Products.Update(product);

//            await _context.SaveChangesAsync();

//            return ServiceResult<InventoryProduct>.SuccessResult(inventoryProduct, "Inventory product updated successfully");
//        }

//        public async Task<ServiceResult<bool>> DeleteInventoryProductAsync(DeleteInventoryProductDto inventoryProductDto)
//        {
//            var inventory = await _context.Inventories.FirstOrDefaultAsync(I => I.InventoryId == inventoryProductDto.InventoryId);

//            if (inventory == null)
//            {
//                return ServiceResult<bool>.ErrorResult("Inventory not found", StatusCodes.Status404NotFound);
//            }

//            var product = await _context.Products.FirstOrDefaultAsync(P => P.ProductId == inventoryProductDto.ProductId);

//            if (product == null)
//            {
//                return ServiceResult<bool>.ErrorResult("Product not found", StatusCodes.Status404NotFound);
//            }

//            var inventoryProduct = await _context.InventoryProducts.FirstOrDefaultAsync(IP => IP.InventoryId == inventoryProductDto.InventoryId && IP.ProductId == inventoryProductDto.ProductId);

//            if (inventoryProduct == null)
//            {
//                return ServiceResult<bool>.ErrorResult("Product not found in this inventory", StatusCodes.Status404NotFound);
//            }

//            _context.InventoryProducts.Remove(inventoryProduct);
//            await _context.SaveChangesAsync();

//            return ServiceResult<bool>.SuccessResult(true, "Inventory product deleted successfully");
//        }

//    }
//}
