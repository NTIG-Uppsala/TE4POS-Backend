using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController : ControllerBase // Inherit from ControllerBase to use HTTP status codes
    {

        public ProductController()
        {

        }

        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            return ProductService.GetAllProducts();
        }
        [HttpGet("{id}")]
        public ActionResult<object> GetProductById(int id)
        {
            var product = ProductService.GetProduct(id);

            if (product == null)
                return NotFound();

            return product;
        }
        [HttpPost]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            return ProductService.Create(product);

        }
        [HttpPut("{id}")]
        public ActionResult<Product> UpdateProduct(int id, Product updatedProduct)
        {
            var existingProduct = ProductService.GetProduct(id);
            if (existingProduct == null)
                return NotFound();
            else
            {
                return ProductService.Update(id, updatedProduct);
            }
        }
        [HttpPost("{id}/stock/add")]
        public ActionResult<object> AddStock(int id, [FromBody] StockUpdate data)
        {
            int amount = data.Amount;
            var existingProduct = ProductService.GetProduct(id);
            if (existingProduct == null)
                return NotFound();
            else
            {
                return ProductService.IncreaseStock(id, amount);
            }
        }

        [HttpPost("{id}/stock/remove")]
        public ActionResult<object> RemoveStock(int id, [FromBody] StockUpdate data)
        {
            int amount = data.Amount;
            var existingProduct = ProductService.GetProduct(id);
            if (existingProduct == null)
                return NotFound();
            else
            {
                return ProductService.DecreaseStock(id, amount);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Product> DeleteProduct(int id)
        {
            var existingProduct = ProductService.GetProduct(id);
            if (existingProduct == null)
                return NotFound();
            else
            {
                ProductService.Delete(id);
                return NoContent();
            }
        }
    }
    public class StockUpdate
    {
        public int Amount { get; set; }
    }
}