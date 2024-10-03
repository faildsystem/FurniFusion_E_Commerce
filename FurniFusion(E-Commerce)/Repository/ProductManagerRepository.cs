//using FurniFusion.Data;
//using FurniFusion.Dtos.ProductManager;
//using FurniFusion.Interfaces;
//using FurniFusion.Models;
//using FurniFusion.Queries;
//using Microsoft.EntityFrameworkCore;

//namespace FurniFusion.Repository
//{
//    public class ProductManagerRepository : IProductManagerRepository
//    {
//        private readonly FurniFusionDbContext _context;

//        public ProductManagerRepository(FurniFusionDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Category> CreateCategoryAsync(CreateCategoryDto categoryDto, string creatorId)
//        {
//            var category = new Category
//            {
//                CategoryName = categoryDto.CategoryName,
//                CreatedAt = DateTime.Now,
//                UpdatedAt = DateTime.Now,
//                CreatedBy = creatorId,
//                UpdatedBy = creatorId
//            };

//            _context.Categories.Add(category);
//            await _context.SaveChangesAsync();
//            return category;
//        }

//        public async Task<Product> CreateProductAsync(CreateProductDto productDto, string creatorId)
//        {
//            var product = new Product
//            {
//                ProductName = productDto.ProductName,
//                ImageUrls = productDto.ImageUrls,
//                Dimensions = productDto.Dimensions,
//                Price = productDto.Price,
//                StockQuantity = productDto.StockQuantity,
//                IsAvailable = productDto.IsAvailable,
//                Weight = productDto.Weight,
//                Color = productDto.Color,
//                Description = productDto.Description,
//                CategoryId = productDto.CategoryId,
//                CreatorId = creatorId
//            };

//            _context.Products.Add(product);
//            await _context.SaveChangesAsync();

//            return product;
//        }

//        public async Task DeleteCategoryAsync(int id)
//        {
//            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
//            if (category == null)
//            {
//                throw new Exception("Category not found");
//            }

//            _context.Categories.Remove(category);
//            await _context.SaveChangesAsync();
//        }

//        public async Task DeleteProductAsync(int id)
//        {
//            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
//            if (product == null)
//            {
//                throw new Exception("Product not found");
//            }

//            _context.Products.Remove(product);
//            await _context.SaveChangesAsync();
//        }

//        public Task<List<Category>> GetAllCategoriesAsync()
//        {
//            return _context.Categories.ToListAsync();
//        }

//        public async Task<List<Product>> GetAllProductsAsync(ProductFilter filter)
//        {
//            var products = _context.Products.AsQueryable();

//            if (filter.Id > 0)
//            {
//                products = products.Where(x => x.ProductId == filter.Id);
//            }

//            if (!string.IsNullOrEmpty(filter.Name))
//            {
//                products = products.Where(x => x.ProductName!.Contains(filter.Name));
//            }

//            if (filter.Price.HasValue)
//            {
//                products = products.Where(x => x.Price == filter.Price);
//            }

//            if (filter.StockQuantity.HasValue)
//            {
//                products = products.Where(x => x.StockQuantity == filter.StockQuantity);
//            }

//            if (filter.IsAvailable.HasValue)
//            {
//                products = products.Where(x => x.IsAvailable == filter.IsAvailable);
//            }

//            if (filter.Weight.HasValue)
//            {
//                products = products.Where(x => x.Weight == filter.Weight);
//            }

//            if (!string.IsNullOrEmpty(filter.Color))
//            {
//                products = products.Where(x => x.Color!.Contains(filter.Color));
//            }

//            if (!string.IsNullOrEmpty(filter.Description))
//            {
//                products = products.Where(x => x.Description!.Contains(filter.Description));
//            }

//            if (filter.CategoryId.HasValue)
//            {
//                products = products.Where(x => x.CategoryId == filter.CategoryId);
//            }

//            if (!string.IsNullOrEmpty(filter.CreatorId))
//            {
//                products = products.Where(x => x.CreatorId == filter.CreatorId);
//            }

//            var skipNumber = (filter.PageNumber - 1) * filter.PageSize;

//            var productsList = await products.Skip(skipNumber).Take(filter.PageSize).ToListAsync();

//            return await products.Skip(skipNumber).Take(filter.PageSize).ToListAsync();
//        }

//        public async Task<Category> UpdateCategoryAsync(UpdateCategoryDto categoryDto, string updatorId)
//        {
//            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryDto.CategoryId);

//            if (category == null)
//            {
//                throw new Exception("Category not found");
//            }

//            category.CategoryName = categoryDto.NewCategoryName;
//            category.UpdatedAt = DateTime.Now;
//            category.UpdatedBy = updatorId;

//            _context.Categories.Update(category);
//            await _context.SaveChangesAsync();
//            return category;
//        }

//        public async Task<Product> UpdateProductAsync(UpdateProductDto productDto)
//        {
//            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productDto.ProductId);

//            if (product == null)
//            {
//                throw new Exception("Product not found");
//            }

//            product.ProductName = productDto.ProductName ?? product.ProductName;
//            product.Dimensions = productDto.Dimensions ?? product.Dimensions;
//            product.Price = productDto.Price ?? product.Price;
//            product.StockQuantity = productDto.StockQuantity ?? product.StockQuantity;
//            product.IsAvailable = productDto.IsAvailable ?? product.IsAvailable;
//            product.Weight = productDto.Weight ?? product.Weight;
//            product.Color = productDto.Color ?? product.Color;
//            product.Description = productDto.Description ?? product.Description;
//            product.CategoryId = productDto.CategoryId ?? product.CategoryId;

//            if (productDto.ImageUrls != null && productDto.ImageUrls.Any())
//            {
//                product.ImageUrls ??= new List<string>();
//                product.ImageUrls.AddRange(productDto.ImageUrls); 
//            }

//            product.UpdatedAt = DateTime.Now;

//            _context.Products.Update(product);
//            await _context.SaveChangesAsync();
//            return product;
            
//        }
//    }
//}
