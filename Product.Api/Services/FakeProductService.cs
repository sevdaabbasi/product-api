using Product.Api.Domain;
using Product.Api.Domain.Interfaces;

namespace Product.Api.Services
{
    public class FakeProductService : IProductService
    {
        private readonly List<ProductEntity> _products;

        public FakeProductService()
        {
            _products = new List<ProductEntity>
            {
                new ProductEntity { Id = 1, Name = "Laptop", Price = 999.99m, Description = "High-performance laptop" },
                new ProductEntity { Id = 2, Name = "Smartphone", Price = 499.99m, Description = "Latest smartphone model" },
                new ProductEntity { Id = 3, Name = "Headphones", Price = 99.99m, Description = "Wireless noise-cancelling headphones" }
            };
        }

        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            return await Task.FromResult(_products);
        }

        public async Task<ProductEntity> GetProductByIdAsync(int id)
        {
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public async Task<ProductEntity> CreateProductAsync(ProductEntity product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            return await Task.FromResult(product);
        }

        public async Task<ProductEntity> UpdateProductAsync(int id, ProductEntity product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
            }
            return await Task.FromResult(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
} 