using Microsoft.AspNetCore.Http.HttpResults;
using StockAPI.Models;

namespace StockAPI.Services;

public static class ProductService
{
    public static List<Product> GetAllProducts() => DatabaseHelper.GetProductsDB();

    public static object GetProduct(int id) => DatabaseHelper.ReadProduct(id);

    public static Product Create(Product Product)
    {
        return DatabaseHelper.CreateProduct(Product);
    }

    public static Product Delete(int id)
    {
        return DatabaseHelper.DeleteProductById(id);
    }

    public static void Update(Product Product)
    {

    }
}