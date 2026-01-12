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

    public static Product Update(int id, Product updatedProduct) => DatabaseHelper.UpdateProduct(id, updatedProduct);

    public static object IncreaseStock(int id, int amount)
    {
        return DatabaseHelper.AddStock(id, amount);
    }
    public static object DecreaseStock(int id, int amount)
    {
        return DatabaseHelper.RemoveStock(id, amount);
    }
}