namespace StockAPI.Models
{
    public class Product
    {
        public int id { get; set; }
        public required string name { get; set; }
        public int category { get; set; }
        public int stock { get; set; }
        public int price { get; set; }
        public string ?created_at { get; set; }
        public string ?updated_at { get; set; }
    }
}
