using Microsoft.AspNetCore.Http.HttpResults;
using StockAPI.Models;

namespace StockAPI.Services;

public static class ProductService
{
    static List<Product> Products { get; }
    static int nextId = 3;
    static ProductService()
    {
        Products = new List<Product>
        {
            new Product { id = 1, name = "Classic Italian", category = 1, stock = 50 },
            new Product { id = 2, name = "Veggie", category = 4, stock = 50 }
        };
    }

    public static List<Product> GetAll() => DatabaseHelper.GetProducts();

    public static Product? Get(int id) => Products.FirstOrDefault(p => p.id == id);

    public static void Add(Product Product)
    {
        Product.id = nextId++;
        Products.Add(Product);
    }

    public static void Delete(int id)
    {
        var Product = Get(id);
        if (Product is null)
            return;

        Products.Remove(Product);
    }

    public static void Update(Product Product)
    {
        var index = Products.FindIndex(p => p.id == Product.id);
        if (index == -1)
            return;

        Products[index] = Product;
    }
}