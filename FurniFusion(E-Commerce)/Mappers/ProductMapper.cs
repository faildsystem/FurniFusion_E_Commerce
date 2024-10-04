using FurniFusion.Models;
using FurniFusion.Dtos.ProductManager.Product;

namespace FurniFusion.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product product)
        {
            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ImageUrls = product.ImageUrls,
                Dimensions = product.Dimensions,
                Weight = product.Weight,
                Colors = product.Colors,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsAvailable = product.IsAvailable,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                CreatedBy = product.CreatedBy,
                UpdatedBy = product.UpdatedBy,
                DiscountId = product.DiscountId,
                CategoryId = product.CategoryId,
                AverageRating = product.AverageRating
            };
        }
    }
}
