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
        public ActionResult<Product> CreateProduct(Product product)
        {
            ProductService.Create(product);
            return product;

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
}