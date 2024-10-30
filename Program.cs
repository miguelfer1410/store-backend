using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyShopAPI.Data;
using MyShopAPI.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configurar MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Adicionar o IMongoClient
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<ConversationService>();
builder.Services.AddScoped<FavoriteService>();

// Configurar CORS para permitir o frontend React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Endereço do frontend React
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Adicionar controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/Uploads"
});

// Habilitar o middleware CORS antes dos endpoints
app.UseCors("AllowReactApp");

// Mapear endpoints
app.MapControllers();

app.Run();
