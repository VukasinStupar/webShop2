using Microsoft.EntityFrameworkCore;
using webShop2.Infrastructure.Data;
using webShop2.repository;
using webShop2.repository.core;
using webShop2.services;
using webShop2.services.core;
using webShop2.mapper;
using webShop2.middleware;
using webShop2.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);
string connectionString =
    Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
builder.Services.AddScoped<IProductValueRepository, ProductValueRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
builder.Services.AddScoped<IProductValueService, ProductValueService>();

builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<CategoryMapper>();
builder.Services.AddScoped<ImageMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<OrderMapper>();
builder.Services.AddScoped<ItemMapper>();
builder.Services.AddScoped<PaymentMapper>();
builder.Services.AddScoped<ProductAttributeMapper>();
builder.Services.AddScoped<ProductValueMapper>();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://webshop2-92tu.onrender.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("ReactFrontend");

app.MapControllers();

app.Run();