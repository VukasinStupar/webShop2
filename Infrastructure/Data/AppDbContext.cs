using Microsoft.EntityFrameworkCore;
using webShop2.model;

namespace webShop2.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<ProductValue> ProductValues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
        .HasOne<Category>()
        .WithMany()
        .HasForeignKey(x => x.ParentId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Image>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Item>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Item>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductAttribute>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductValue>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProductValue>()
            .HasOne<ProductAttribute>()
            .WithMany()
            .HasForeignKey(x => x.ProductAttributeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);





        modelBuilder.Entity<Product>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(x => x.Total)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Item>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .Property(x => x.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProductValue>()
            .Property(x => x.Decimal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();




        modelBuilder.Entity<User>()
            .Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();


        modelBuilder.Entity<Product>()
            .Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(x => x.Description)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();


        modelBuilder.Entity<Category>()
            .Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();


        modelBuilder.Entity<Image>()
            .Property(x => x.Url)
            .HasMaxLength(1000)
            .IsRequired();


        modelBuilder.Entity<Order>()
            .Property(x => x.Number)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(x => x.CustomerName)
            .HasMaxLength(200)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(x => x.CustomerEmail)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(x => x.CustomerPhone)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(x => x.Address)
            .HasMaxLength(300)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(x => x.City)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(x => x.PostalCode)
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<Order>()
            .Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();


        modelBuilder.Entity<Payment>()
            .Property(x => x.Currency)
            .HasMaxLength(10)
            .IsRequired();

        modelBuilder.Entity<Payment>()
            .Property(x => x.Provider)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Payment>()
            .Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Payment>()
            .Property(x => x.TransactionId)
            .HasMaxLength(200);

        modelBuilder.Entity<Payment>()
            .Property(x => x.FailureReason)
            .HasMaxLength(500);


        modelBuilder.Entity<ProductAttribute>()
            .Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();


        modelBuilder.Entity<ProductValue>()
            .Property(x => x.Text)
            .HasMaxLength(1000);
    }
}