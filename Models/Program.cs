using StockAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

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
