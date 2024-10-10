using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Product;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FurniFusion.Services
{
    public class ProductService : IProductService
    {
        private readonly FurniFusionDbContext _context;

        public ProductService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Product>>> GetAllProductsAsync(ProductFilter filter)
        {
            var products = _context.Products.Include(c => c.Category).Include(d => d.Discount).AsQueryable();

            if (filter.Id > 0)
            {
                products = products.Where(x => x.ProductId == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                products = products.Where(x => x.ProductName!.Contains(filter.Name));
            }

            if (filter.Price.HasValue)
            {
                products = products.Where(x => x.Price == filter.Price);
            }

            if (filter.StockQuantity.HasValue)
            {
                products = products.Where(x => x.StockQuantity == filter.StockQuantity);
            }

            if (filter.IsAvailable.HasValue)
            {
                products = products.Where(x => x.IsAvailable == filter.IsAvailable);
            }

            if (filter.Weight.HasValue)
            {
                products = products.Where(x => x.Weight == filter.Weight);
            }

            if (!string.IsNullOrEmpty(filter.Color))
            {
                products = products.Where(x => x.Colors!.Contains(filter.Color));
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                products = products.Where(x => x.Description!.Contains(filter.Description));
            }

            if (filter.CategoryId.HasValue)
            {
                products = products.Where(x => x.CategoryId == filter.CategoryId);
            }

            if (!string.IsNullOrEmpty(filter.CreatedBy))
            {
                products = products.Where(x => x.CreatedBy == filter.CreatedBy);
            }

            if (!string.IsNullOrEmpty(filter.UpdatedBy))
            {
                products = products.Where(x => x.UpdatedBy == filter.UpdatedBy);
            }


            var skipNumber = (filter.PageNumber - 1) * filter.PageSize;

            var productsList = await products.Skip(skipNumber).Take(filter.PageSize).ToListAsync();

            var result = await products.Skip(skipNumber).Take(filter.PageSize).ToListAsync();

            return ServiceResult<List<Product>>.SuccessResult(result);
        }

        public async Task<ServiceResult<Product>> CreateProductAsync(CreateProductDto productDto, string creatorId)
        {
            var category = await _context.Categories
                             .FirstOrDefaultAsync(c => c.CategoryId == productDto.CategoryId);

            if (category == null)
            {
                return ServiceResult<Product>.ErrorResult("Category not found", StatusCodes.Status404NotFound);
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductName == productDto.ProductName &&
                    p.CategoryId == productDto.CategoryId &&
                    p.Price == productDto.Price &&
                    p.Dimensions == productDto.Dimensions &&
                    p.Colors == productDto.Colors
                );

            if (product != null) {
                return ServiceResult<Product>.ErrorResult("Product with the same attributes already exists", StatusCodes.Status409Conflict);
            }

            var productImagesUrls = await UploadProductImagesAsync(productDto.Images!, productDto.ProductName!);

            var newProduct = new Product
            {
                ProductName = productDto.ProductName!,
                ImageUrls = productImagesUrls,
                Dimensions = productDto.Dimensions,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                IsAvailable = productDto.IsAvailable,
                Weight = productDto.Weight,
                Colors = productDto.Colors,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId,
                CreatedBy = creatorId,
                UpdatedBy = creatorId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return ServiceResult<Product>.SuccessResult(newProduct, "Product created successfully", StatusCodes.Status201Created);
        }

        public async Task<ServiceResult<Product>> UpdateProductAsync(UpdateProductDto productDto, string updatorId)
        {
            var result = await GetProductByIdAsync(productDto.ProductId);

            if (result == null || result.Data == null)
            {
                return ServiceResult<Product>.ErrorResult("Product not found", StatusCodes.Status404NotFound);
            }

            result.Data.ProductName = productDto.ProductName ?? result.Data.ProductName;
            result.Data.Dimensions = productDto.Dimensions ?? result.Data.Dimensions;
            result.Data.Price = productDto.Price ?? result.Data.Price;
            result.Data.StockQuantity = productDto.StockQuantity ?? result.Data.StockQuantity;
            result.Data.IsAvailable = productDto.IsAvailable ?? result.Data.IsAvailable;
            result.Data.Weight = productDto.Weight ?? result.Data.Weight;
            result.Data.Description = productDto.Description ?? result.Data.Description;
            result.Data.CategoryId = productDto.CategoryId ?? result.Data.CategoryId;
            result.Data.UpdatedBy = updatorId;
            result.Data.UpdatedAt = DateTime.Now;

      
            if (productDto.Colors != null && productDto.Colors.Any())
            {
                result.Data.Colors!.AddRange(productDto.Colors);
            }

            if (productDto.Images != null && productDto.Images.Any())
            {
                RemoveProductImagesFolder(result.Data.ProductName);
                var productImagesUrls = await UploadProductImagesAsync(productDto.Images!, result.Data.ProductName!);

                result.Data.ImageUrls = productImagesUrls;
            }

            _context.Products.Update(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<Product>.SuccessResult(result.Data, "Product updated successfully", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<bool>> DeleteProductAsync(int id)
        {
            var result = await GetProductByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Product not found", StatusCodes.Status404NotFound);
            }

            _context.Products.Remove(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Product deleted successfully");

        }

        public async Task<ServiceResult<Product>> GetProductByIdAsync(int? id)
        {
            var product = await _context.Products.Include(c => c.Category).Include(d => d.Discount).FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return ServiceResult<Product>.ErrorResult("Product not found", StatusCodes.Status404NotFound);
            }

            return ServiceResult<Product>.SuccessResult(product);
        }

        public async Task<ServiceResult<Product>> ApplyDiscountToProductAsync(int productId, int discountId)
        {

            var result = await GetProductByIdAsync(productId);

            if (result == null || result.Data == null)
            {
                return ServiceResult<Product>.ErrorResult($"Product with ID {productId} not found.", StatusCodes.Status404NotFound);
            }

            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);

            if (discount == null)
            {
                return ServiceResult<Product>.ErrorResult($"Discount with ID {discountId} not found.", StatusCodes.Status404NotFound);
            }

            if (discount.IsActive == false || discount.ValidFrom > DateOnly.FromDateTime(DateTime.Now) || discount.ValidTo < DateOnly.FromDateTime(DateTime.Now))
            {
                return ServiceResult<Product>.ErrorResult("Discount is not active or valid", StatusCodes.Status400BadRequest);
            }

            result.Data.DiscountId = discount.DiscountId;

            result.Data.Discount = discount;

            discount.Products.Add(result.Data);

            _context.Products.Update(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<Product>.SuccessResult(result.Data, "Discount applied to product successfully", StatusCodes.Status200OK);
        }

        public async Task<List<string>> UploadProductImagesAsync(List<IFormFile> productImages, string productName)
        {
            // List to store the URLs of the uploaded images
            var uploadedImageUrls = new List<string>();

            // Ensure the folder path exists
            var folderPath = Path.Combine("wwwroot", "images", "products", productName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var file in productImages)
            {
                // Get the file extension
                var extension = Path.GetExtension(file.FileName).ToLower();

                // Generate a unique file name for each image
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(folderPath, uniqueFileName);

                // Save the file to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Generate the image URL and add it to the list
                uploadedImageUrls.Add(filePath);

            }
            // Return the list of uploaded image URLs
            return uploadedImageUrls;
        }

        private void RemoveProductImagesFolder(string productName)
        {
            var folderPath = Path.Combine("wwwroot", "images", "products", productName);

            Console.WriteLine(folderPath + "\n\n\n\n\n\n");

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

        }
    }
}
