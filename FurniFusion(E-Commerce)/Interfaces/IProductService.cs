using FurniFusion.Models;
using FurniFusion.Queries;
using FurniFusion.Dtos.ProductManager.Product;

namespace FurniFusion.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<List<Product>>> GetAllProductsAsync(ProductFilter filter);
        Task<ServiceResult<Product>> GetProductByIdAsync(int? id);
        Task<ServiceResult<Product>> CreateProductAsync(CreateProductDto productDto, string creatorId);
        Task<ServiceResult<Product>> UpdateProductAsync(UpdateProductDto productDto, string updatorId);
        Task<ServiceResult<bool>> DeleteProductAsync(int id);
        Task<ServiceResult<Product>> ApplyDiscountToProductAsync(int productId, int discountId);

    }
}
