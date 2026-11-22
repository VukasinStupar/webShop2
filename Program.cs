
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using System.Text;

using webShop2.Infrastructure.Data;
using webShop2.repository;
using webShop2.repository.core;
using webShop2.services;
using webShop2.services.core;
using webShop2.mapper;
using webShop2.middleware;
using webShop2.Infrastructure.Storage;
using webShop2.model;
using webShop2.model.enums;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();

// DATABASE

string connectionString =
    Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string is not configured.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// IDENTITY

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 12;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;

        options.Lockout.MaxFailedAccessAttempts = 5;

        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(15);

        options.Lockout.AllowedForNewUsers = true;
    })
    .AddRoles<UserRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager();

// JWT AUTHENTICATION

string secretKey =
    builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException(
        "JWT SecretKey is not configured.");

string issuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "JWT Issuer is not configured.");

string audience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "JWT Audience is not configured.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey)),

                RoleClaimType = ClaimTypes.Role,

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

// REPOSITORIES

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
builder.Services.AddScoped<IProductValueRepository, ProductValueRepository>();

// UNIT OF WORK

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// SERVICES

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IProductAttributeService, ProductAttributeService>();
builder.Services.AddScoped<IProductValueService, ProductValueService>();

// MAPPERS

builder.Services.AddScoped<ProductMapper>();
builder.Services.AddScoped<CategoryMapper>();
builder.Services.AddScoped<ImageMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<OrderMapper>();
builder.Services.AddScoped<ItemMapper>();
builder.Services.AddScoped<PaymentMapper>();
builder.Services.AddScoped<ProductAttributeMapper>();
builder.Services.AddScoped<ProductValueMapper>();

// AUTH SERVICES

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// CLOUDINARY

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary"));

builder.Services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();

// CORS

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

// CONTROLLERS

builder.Services.AddControllers();

var app = builder.Build();

// APPLY MIGRATIONS
using (IServiceScope migrationScope = app.Services.CreateScope())
{
    AppDbContext db =
        migrationScope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();
}

// DATABASE ROLE SEEDING

using (IServiceScope scope = app.Services.CreateScope())
{
    RoleManager<UserRole> roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<UserRole>>();

    bool adminRoleExists =
        await roleManager.RoleExistsAsync(UserRoles.Admin);

    if (!adminRoleExists)
    {
        IdentityResult result =
            await roleManager.CreateAsync(
                new UserRole
                {
                    Name = UserRoles.Admin
                });

        if (!result.Succeeded)
        {
            string errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            throw new InvalidOperationException(
                "Failed to create Admin role: " + errors);
        }
    }
}

// MIDDLEWARE

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("ReactFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// ENDPOINT DEBUGGING

IEnumerable<EndpointDataSource> endpointDataSources =
    app.Services.GetRequiredService<IEnumerable<EndpointDataSource>>();

foreach (EndpointDataSource dataSource in endpointDataSources)
{
    foreach (Endpoint endpoint in dataSource.Endpoints)
    {
        RouteEndpoint? routeEndpoint = endpoint as RouteEndpoint;

        Console.WriteLine(
            $"REGISTERED ENDPOINT: {endpoint.DisplayName}");

        if (routeEndpoint != null)
        {
            Console.WriteLine(
                $"ROUTE: {routeEndpoint.RoutePattern.RawText}");
        }
    }
}



app.Run();
