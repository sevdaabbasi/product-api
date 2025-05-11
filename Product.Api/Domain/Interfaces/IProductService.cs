using Product.Api.Domain;

namespace Product.Api.Domain.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductEntity>> GetAllProductsAsync();
        Task<ProductEntity> GetProductByIdAsync(int id);
        Task<ProductEntity> CreateProductAsync(ProductEntity product);
        Task<ProductEntity> UpdateProductAsync(int id, ProductEntity product);
        Task<bool> DeleteProductAsync(int id);
    }
} 