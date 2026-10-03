using Microsoft.EntityFrameworkCore;
using webShop2.Infrastructure.Data;
using webShop2.repository;
using webShop2.repository.core;
using webShop2.services;
using webShop2.services.core;
using webShop2.mapper;
using webShop2.middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));


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

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();