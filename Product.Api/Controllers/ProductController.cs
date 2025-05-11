using Microsoft.AspNetCore.Mvc;
using Product.Api.Attributes;
using Product.Api.Domain;
using Product.Api.Domain.Interfaces;

namespace Product.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authenticate]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductEntity>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductEntity>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<ProductEntity>>> GetFiltered([FromQuery] string name = null, [FromQuery] string sortBy = "name")
        {
            var products = await _productService.GetAllProductsAsync();
            var query = products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            query = sortBy.ToLower() switch
            {
                "price" => query.OrderBy(p => p.Price),
                "stock" => query.OrderBy(p => p.Stock),
                _ => query.OrderBy(p => p.Name)
            };

            return Ok(query.ToList());
        }

        [HttpPost]
        public async Task<ActionResult<ProductEntity>> Create(ProductEntity product)
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductEntity product)
        {
            var updatedProduct = await _productService.UpdateProductAsync(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdate(int id, [FromBody] Dictionary<string, object> updates)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            foreach (var update in updates)
            {
                var property = typeof(ProductEntity).GetProperty(update.Key, 
                    System.Reflection.BindingFlags.IgnoreCase | 
                    System.Reflection.BindingFlags.Public | 
                    System.Reflection.BindingFlags.Instance);
                
                if (property != null)
                {
                    property.SetValue(existingProduct, Convert.ChangeType(update.Value, property.PropertyType));
                }
            }

            var updatedProduct = await _productService.UpdateProductAsync(id, existingProduct);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
} 