using StockAPI.Models;
using System.Data.SQLite;

namespace StockAPI
{
    public static class DatabaseHelper
    {

        private static readonly string filePath = "Databases/Database.db";
        private static readonly string connectionString = @"Data Source=" + filePath + ";Version=3";

        public static string currentConnectionString = "";

        public static List<Product>? AllProducts;

        public static string GetConnectionString()
        {
            currentConnectionString = connectionString;
            return currentConnectionString;
        }
        public static void InitializeDatabase()
        {

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                SQLiteConnection.CreateFile(filePath);
                using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
                {
                    connection.Open();

                    // Creates the database tables
                    string createProductsQuery = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Price INTEGER NOT NULL,
                        Category INTEGER NOT NULL,
                        Stock INTEGER NOT NULL,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT NOT NULL
                    )";

                    string createReceiptsQuery = @"
                    CREATE TABLE IF NOT EXISTS Receipts (
                        ReceiptNumber INTEGER PRIMARY KEY AUTOINCREMENT,
                        ArticleCount INTEGER NOT NULL,
                        ReceiptTotal INTEGER NOT NULL,
                        Subtotal FLOAT NOT NULL,
                        Saletax FLOAT NOT NULL,
                        PdfFormattedTime TEXT NOT NULL,
                        Time TEXT NOT NULL
                    )";

                    string createReceiptProductsQuery = @"
                    CREATE TABLE IF NOT EXISTS ReceiptProducts(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ReceiptNumber INTEGER NOT NULL,
                        ProductId INTEGER NOT NULL,
                        Quantity INTEGER NOT NULL,
                        UnitPrice INTEGER NOT NULL,
                        TotalPrice INTEGER NOT NULL,
                        FOREIGN KEY (ReceiptNumber) REFERENCES Receipts(ReceiptNumber),
                        FOREIGN KEY (ProductId) REFERENCES Products(Id)
                    )";

                    string createCategoriesQuery = @"
                    CREATE TABLE IF NOT EXISTS ProductCategories(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CategoryName TEXT NOT NULL
                    )";

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText = createProductsQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createReceiptsQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createReceiptProductsQuery;
                        command.ExecuteNonQuery();

                        command.CommandText = createCategoriesQuery;
                        command.ExecuteNonQuery();
                    }
                }
                AddCategories(GetConnectionString());
                AddProducts(GetConnectionString());
            }
        }


        public static void AddCategories(string connectionString)
        {
            var Categories = new[]
            {
                new { CategoryName = "Varma drycker"},
                new { CategoryName = "Kalla drycker"},
                new { CategoryName = "Bakverk"},
                new { CategoryName = "Enkel mat" }
            };
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction())
                using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO ProductCategories (CategoryName) 
                                           VALUES (@category)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@category"));

                    foreach (var category in Categories)
                    {
                        cmd.Parameters["@category"].Value = category.CategoryName;
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
        }

        public static void AddProducts(string connectionString)
        {
            var products = new[]
            {
                new { Name = "Bryggkaffe (liten)", Price = 28, Category = 1, Stock = 100 },
                new { Name = "Bryggkaffe (stor)", Price = 34, Category = 1, Stock = 100 },
                new { Name = "Cappuccino", Price = 42, Category = 1, Stock = 100 },
                new { Name = "Latte", Price = 46, Category = 1, Stock = 100 },
                new { Name = "Varm choklad med grädde", Price = 45, Category = 1, Stock = 100 },
                new { Name = "Te (svart, grönt eller örtte)", Price = 32, Category = 1, Stock = 100 },

                new { Name = "Islatte", Price = 48, Category = 2, Stock = 100 },
                new { Name = "Ischai", Price = 46, Category = 2, Stock = 100 },
                new { Name = "Läsk (33 cl)", Price = 22, Category = 2, Stock = 100 },
                new { Name = "Mineralvatten", Price = 20, Category = 2, Stock = 100 },
                new { Name = "Smoothie (jordgubb & banan)", Price = 55, Category = 2, Stock = 100 },
                new { Name = "Färskpressad apelsinjuice", Price = 49, Category = 2, Stock = 100 },

                new { Name = "Kanelbulle", Price = 25, Category = 3, Stock = 100 },
                new { Name = "Chokladboll", Price = 18, Category = 3, Stock = 100 },
                new { Name = "Morotskaka (bit)", Price = 38, Category = 3, Stock = 100 },
                new { Name = "Cheesecake (bit)", Price = 42, Category = 3, Stock = 100 },
                new { Name = "Croissant", Price = 26, Category = 3, Stock = 100 },
                new { Name = "Muffins (blåbär)", Price = 28, Category = 3, Stock = 100 },

                new { Name = "Smörgås (ost & skinka)", Price = 38, Category = 4, Stock = 100 },
                new { Name = "Räksmörgås", Price = 69, Category = 4, Stock = 100 },
                new { Name = "Panini (kyckling & pesto)", Price = 58, Category = 4, Stock = 100 },
                new { Name = "Soppa med bröd", Price = 65, Category = 4, Stock = 100 },
                new { Name = "Quinoasallad", Price = 72, Category = 4, Stock = 100 },
            };

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var tx = connection.BeginTransaction())
                using (SQLiteCommand cmd = new SQLiteCommand(@"INSERT INTO Products (Name, Price, Category, Stock, CreatedAt, UpdatedAt) 
                                           VALUES (@name, @price, @category, @stock, @created_at, @updated_at)", connection, tx))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@name"));
                    cmd.Parameters.Add(new SQLiteParameter("@price"));
                    cmd.Parameters.Add(new SQLiteParameter("@category"));
                    cmd.Parameters.Add(new SQLiteParameter("@stock"));
                    cmd.Parameters.AddWithValue("@created_at", DateTime.Now.ToString("yyyy-MM-ddT HH:mm:ssZ"));
                    cmd.Parameters.AddWithValue("@updated_at", DateTime.Now.ToString("yyyy-MM-ddT HH:mm:ssZ"));

                    foreach (var product in products)
                    {
                        cmd.Parameters["@name"].Value = product.Name;
                        cmd.Parameters["@price"].Value = product.Price;
                        cmd.Parameters["@category"].Value = product.Category;
                        cmd.Parameters["@stock"].Value = product.Stock;
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
            }
        }

        public static int GetProductId(string productName)
        {
            string selectProductIdQuery = "SELECT Id FROM Products WHERE Name = @productName";

            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(selectProductIdQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@productName", productName);

                    // ExecuteScalar returns the first column of the first row where the query matches
                    object result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int productId))
                    {
                        return productId;
                    }
                    else
                    {
                        throw new Exception("Product not found or invalid Id");
                    }
                }
            }
        }
        public static List<Product> GetProductsDB()
        {
            AllProducts = new List<Product>();
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = "SELECT * FROM Products";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AllProducts.Add(new Product()
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.GetString(reader.GetOrdinal("Name")),
                            price = reader.GetInt32(reader.GetOrdinal("Price")),
                            category = reader.GetInt32(reader.GetOrdinal("Category")),
                            stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                            created_at = reader.GetString(reader.GetOrdinal("CreatedAt")),
                            updated_at = reader.GetString(reader.GetOrdinal("UpdatedAt"))
                        });
                    }
                }
            }
            return AllProducts;
        }
        public static Product GetProductById(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                string query = "SELECT * FROM Products WHERE Id = @id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null; 

                        return new Product()
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.GetString(reader.GetOrdinal("Name")),
                            price = reader.GetInt32(reader.GetOrdinal("Price")),
                            category = reader.GetInt32(reader.GetOrdinal("Category")),
                            stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                            created_at = reader.GetString(reader.GetOrdinal("CreatedAt")),
                            updated_at = reader.GetString(reader.GetOrdinal("UpdatedAt"))
                        };
                    }
                }
            }
        }

        public static Product AddStock(int id, int addedStock)
        {
            using var connection = new SQLiteConnection(GetConnectionString());
            connection.Open();

            using var tx = connection.BeginTransaction();

            int currentStock;

            using (var selectCmd = new SQLiteCommand(
                "SELECT Stock FROM Products WHERE Id = @id",
                connection, tx))
            {
                selectCmd.Parameters.AddWithValue("@id", id);

                using var reader = selectCmd.ExecuteReader();

                if (!reader.Read())
                    return null;

                currentStock = reader.GetInt32(reader.GetOrdinal("Stock"));
            }

            int newStock = currentStock + addedStock;

            using (var updateCmd = new SQLiteCommand(
                "UPDATE Products SET Stock = @stock, UpdatedAt = @updatedAt WHERE Id = @id",
                connection, tx))
            {
                updateCmd.Parameters.AddWithValue("@stock", newStock);
                updateCmd.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                updateCmd.Parameters.AddWithValue("@id", id);
                updateCmd.ExecuteNonQuery();
            }

            tx.Commit();

            return GetProductById(id);
        }


        public static object RemoveStock(int id, int removedStock)
        {
            using var connection = new SQLiteConnection(GetConnectionString());
            connection.Open();

            using var tx = connection.BeginTransaction();

            int currentStock;

            using (var selectCmd = new SQLiteCommand(
                "SELECT Stock FROM Products WHERE Id = @id",
                connection, tx))
            {
                selectCmd.Parameters.AddWithValue("@id", id);

                using var reader = selectCmd.ExecuteReader();

                if (!reader.Read())
                    return null;

                currentStock = reader.GetInt32(reader.GetOrdinal("Stock"));
            }

            int newStock = currentStock - removedStock;

            using (var updateCmd = new SQLiteCommand(
                "UPDATE Products SET Stock = @stock, UpdatedAt = @updatedAt WHERE Id = @id",
                connection, tx))
            {
                updateCmd.Parameters.AddWithValue("@stock", newStock);
                updateCmd.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                updateCmd.Parameters.AddWithValue("@id", id);
                updateCmd.ExecuteNonQuery();
            }

            tx.Commit();

            return GetProductById(id);
        }

        public static object ReadProduct(int id)
        {
            try
            {
                object result;

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand($"SELECT * FROM products WHERE id = {id}", connection))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();

                        result = new
                        {
                            id = id,
                            name = reader.GetString(reader.GetOrdinal("Name")),
                            category = reader.GetInt32(reader.GetOrdinal("Category")),
                            price = reader.GetFloat(reader.GetOrdinal("Price")),
                            stock = reader.GetInt32(reader.GetOrdinal("Stock"))
                        };
                    }
                }
                return result;
            } catch (Exception ex)
            {
                return null;
            }
        }

        public static Product CreateProduct(Product newProduct)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = @"
                INSERT INTO Products (Name, Price, Category, Stock, CreatedAt, UpdatedAt)
                VALUES (@name, @price, @category, @stock, @created_at, @updated_at);
                SELECT last_insert_rowid();";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", newProduct.name);
                    command.Parameters.AddWithValue("@price", newProduct.price);
                    command.Parameters.AddWithValue("@category", newProduct.category);
                    command.Parameters.AddWithValue("@stock", newProduct.stock);
                    command.Parameters.AddWithValue("@created_at", DateTime.Now.ToString("yyyy-MM-ddT HH:mm:ssZ"));
                    command.Parameters.AddWithValue("@updated_at", DateTime.Now.ToString("yyyy-MM-ddT HH:mm:ssZ"));
                    long newId = (long)command.ExecuteScalar();
                    newProduct.id = (int)newId;
                }
            }
            return GetProductById(newProduct.id);
        }

        public static Product UpdateProduct(int id, Product updatedProduct)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();
                string query = @"
                UPDATE Products
                SET Name = @name,
                    Price = @price,
                    Category = @category,
                    Stock = @stock,
                    UpdatedAt = @updated_at
                WHERE Id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", updatedProduct.name);
                    command.Parameters.AddWithValue("@price", updatedProduct.price);
                    command.Parameters.AddWithValue("@category", updatedProduct.category);
                    command.Parameters.AddWithValue("@stock", updatedProduct.stock);
                    command.Parameters.AddWithValue("@updated_at", DateTime.Now.ToString("yyyy-MM-ddT HH:mm:ssZ"));
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
            return GetProductById(id);
        }

        public static Product DeleteProductById(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(GetConnectionString()))
            {
                connection.Open();

                string query = "DELETE FROM Products WHERE Id = @id";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();

                }
            }
            return GetProductById(id);
        }
    }
}
