using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Product;

namespace FurniFusion.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync(ProductFilter filter);
        Task<Product> CreateProductAsync(CreateProductDto productDto, string creatorId);
        Task<Product> UpdateProductAsync(UpdateProductDto productDto, string updatorId);
        Task DeleteProductAsync(int id);
    }
}
