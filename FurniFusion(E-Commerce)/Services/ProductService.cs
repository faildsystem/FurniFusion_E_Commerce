using FurniFusion.Data;
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

            if (product != null) {
                throw new Exception("Product with the same attributes already exists");
            }

            var newProduct = new Product
            {
                ProductName = productDto.ProductName!,
                ImageUrls = productDto.ImageUrls,
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

            if (productDto.Colors!.Any())
            {
                product.Colors!.AddRange(productDto.Colors!);
            }

            if (productDto.ImageUrls!.Any())
            {
                product.ImageUrls!.AddRange(productDto.ImageUrls!);
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
    }
}
