using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class ProductServiceUnitTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProductService _service;

    public ProductServiceUnitTests()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _unitOfWorkMock =
            new Mock<IUnitOfWork>();

        _unitOfWorkMock
            .Setup(x => x.Products)
            .Returns(_productRepositoryMock.Object);

        _service =
            new ProductService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
    {
        Product product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 1200,
            CategoryId = 10
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        Product? result =
            await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(product, result);

        _productRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Product?)null);

        Product? result =
            await _service.GetByIdAsync(1);

        Assert.Null(result);

        _productRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        List<Product> products =
        [
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Gaming laptop",
                Price = 1200,
                CategoryId = 10
            },
            new Product
            {
                Id = 2,
                Name = "Phone",
                Description = "Smartphone",
                Price = 800,
                CategoryId = 10
            }
        ];

        _productRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(products);

        List<Product> result =
            await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(products, result);

        _productRepositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_AddsProductAndSavesChanges()
    {
        Product product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 1200,
            CategoryId = 10
        };

        _productRepositoryMock
            .Setup(x => x.AddAsync(product))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Product result =
            await _service.CreateAsync(product);

        Assert.Equal(product, result);

        _productRepositoryMock.Verify(
            x => x.AddAsync(product),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProduct_WhenProductExists()
    {
        Product existingProduct = new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Old description",
            Price = 1000,
            CategoryId = 10
        };

        Product updateProduct = new Product
        {
            Id = 1,
            Name = "Gaming Laptop",
            Description = "New description",
            Price = 1500,
            CategoryId = 20
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingProduct);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Product? result =
            await _service.UpdateAsync(1, updateProduct);

        Assert.NotNull(result);

        Assert.Equal("Gaming Laptop", result.Name);
        Assert.Equal("New description", result.Description);
        Assert.Equal(1500, result.Price);
        Assert.Equal(20, result.CategoryId);

        _productRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.Update(existingProduct),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        Product updateProduct = new Product
        {
            Id = 1,
            Name = "Gaming Laptop",
            Description = "New description",
            Price = 1500,
            CategoryId = 20
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Product?)null);

        Product? result =
            await _service.UpdateAsync(1, updateProduct);

        Assert.Null(result);

        _productRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.Update(It.IsAny<Product>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesProduct_WhenProductExists()
    {
        Product product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Gaming laptop",
            Price = 1200,
            CategoryId = 10
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        bool result =
            await _service.DeleteAsync(1);

        Assert.True(result);

        _productRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.Delete(product),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenProductDoesNotExist()
    {
        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Product?)null);

        bool result =
            await _service.DeleteAsync(1);

        Assert.False(result);

        _productRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Product>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }
}