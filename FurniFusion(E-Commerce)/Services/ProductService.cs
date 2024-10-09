﻿using FurniFusion.Data;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Product;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class ProductService : IProductService
    {
        private readonly FurniFusionDbContext _context;

        public ProductService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync(ProductFilter filter)
        {
            var products = _context.Products.AsQueryable();

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

            return await products.Skip(skipNumber).Take(filter.PageSize).ToListAsync();
        }

        public async Task<Product> CreateProductAsync(CreateProductDto productDto, string creatorId)
        {
            var category = await _context.Categories
        .FirstOrDefaultAsync(c => c.CategoryId == productDto.CategoryId);

            if (category == null)
            {
                throw new Exception("Category does not exist");
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductName == productDto.ProductName &&
                    p.CategoryId == productDto.CategoryId &&
                    p.Price == productDto.Price &&
                    p.Dimensions == productDto.Dimensions &&
                    p.Colors == productDto.Colors
                );

            if (product != null)
            {
                throw new Exception("Product with the same attributes already exists");
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

            return newProduct;
        }

        public async Task<Product> UpdateProductAsync(UpdateProductDto productDto, string updatorId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productDto.ProductId);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            if (productDto.Images != null && productDto.Images.Any())
            {
                RemoveProductImagesFolder(product.ProductName);
                var productImagesUrls = await UploadProductImagesAsync(productDto.Images!, product.ProductName!);

                product.ImageUrls = productImagesUrls;
            }

            product.ProductName = productDto.ProductName ?? product.ProductName;
            product.Dimensions = productDto.Dimensions ?? product.Dimensions;
            product.Price = productDto.Price ?? product.Price;
            product.StockQuantity = productDto.StockQuantity ?? product.StockQuantity;
            product.IsAvailable = productDto.IsAvailable ?? product.IsAvailable;
            product.Weight = productDto.Weight ?? product.Weight;
            product.Description = productDto.Description ?? product.Description;
            product.CategoryId = productDto.CategoryId ?? product.CategoryId;
            product.UpdatedBy = updatorId;
            product.UpdatedAt = DateTime.Now;

            if (productDto.Colors != null && productDto.Colors!.Any())
            {
                product.Colors!.AddRange(productDto.Colors!);
            }


            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;

        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            return product;
        }

        public async Task<Product> ApplyDiscountToProduct(int productId, int discountId)
        {
            // Retrieve the product
            var product = await GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception($"Product with ID {productId} not found.");
            }

            // Retrieve the discount
            var discount = await _context.Discounts.FirstOrDefaultAsync(d => d.DiscountId == discountId);
            if (discount == null)
            {
                throw new Exception($"Discount with ID {discountId} not found.");
            }

            // Check if discount is active and valid
            if (discount.IsActive == false || discount.ValidFrom > DateOnly.FromDateTime(DateTime.Now) || discount.ValidTo < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new Exception("Discount is not valid or active.");
            }

            // Apply discount to product
            product.DiscountId = discount.DiscountId;

            var discountUnit = await _context.DiscountUnits.FirstOrDefaultAsync(du => du.UnitId == discount.DiscountUnitId);

            // Update the product price
            if (discount.DiscountValue.HasValue)
            {
                if (discountUnit!.UnitName == "%")
                {
                    var discountTotalValue = product.Price * (discount.DiscountValue.Value / 100);

                    product.Price -= (discountTotalValue <= discount.MaxAmount!.Value) ? discountTotalValue : discount.MaxAmount.Value;
                }
                else if (discountUnit.UnitName == "$")
                {
                    product.Price -= discount.DiscountValue.Value;
                }
            }

            // Update product in database
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
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

            Console.WriteLine(folderPath+"\n\n\n\n\n\n");

            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

        }

    }
}
