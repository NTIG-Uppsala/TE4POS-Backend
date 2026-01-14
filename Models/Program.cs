using StockAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

builder.WebHost.UseUrls("http://0.0.0.0:3000");

DatabaseHelper.InitializeDatabase();

if (app.Environment.IsDevelopment())
{

}

app.UseDefaultFiles();
app.UseStaticFiles();
app.MapStaticAssets().ShortCircuit();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
