using Microsoft.AspNetCore.Mvc;
using StockAPI.Models;
using System;
using StockAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/v1/products")]
    public class ProductController
    {

        public ProductController()
        {

        }

        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            return ProductService.GetAll();
        }
    }
}