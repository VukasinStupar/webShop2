using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System.Collections.Generic;
using webShop2.Infrastructure.Storage;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;
using Xunit;

namespace webShop2.Tests;

public class ProductServiceUnitTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProductService _service;

    private readonly Mock<IImageStorageService> _imageStorageMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock;

    public ProductServiceUnitTests()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _unitOfWorkMock =
            new Mock<IUnitOfWork>();

        _imageStorageMock =
            new Mock<IImageStorageService>();

        _imageRepositoryMock =
            new Mock<IImageRepository>();

        _unitOfWorkMock
            .Setup(x => x.Products)
            .Returns(_productRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(x => x.Images)
            .Returns(_imageRepositoryMock.Object);

        _service = new ProductService(
            _unitOfWorkMock.Object,
            _imageStorageMock.Object);
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
            .Setup(x => x.GetByIdWithImagesAsync(1))
            .ReturnsAsync(product);

        Product? result =
            await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(product, result);

        _productRepositoryMock.Verify(
            x => x.GetByIdWithImagesAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenProductDoesNotExist()
    {
        _productRepositoryMock
            .Setup(x => x.GetByIdWithImagesAsync(1))
            .ReturnsAsync((Product?)null);

        Product? result =
            await _service.GetByIdAsync(1);

        Assert.Null(result);

        _productRepositoryMock.Verify(
            x => x.GetByIdWithImagesAsync(1),
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
            .Setup(x => x.GetAllWithImagesAsync())
            .ReturnsAsync(products);

        List<Product> result =
            await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(products, result);

        _productRepositoryMock.Verify(
            x => x.GetAllWithImagesAsync(),
            Times.Once);
    }

    //    [Fact]
    //    public async Task CreateAsync_AddsProductAndSavesChanges()
    //    {
    //        Product product = new Product
    //        {
    //            Id = 1,
    //            Name = "Laptop",
    //            Description = "Gaming laptop",
    //            Price = 1200,
    //            CategoryId = 10
    //        };

    //        Mock<ICategoryRepository> categoryRepositoryMock =
    //            new Mock<ICategoryRepository>();

    //        categoryRepositoryMock
    //            .Setup(x => x.GetByIdAsync(10))
    //            .ReturnsAsync(new Category
    //            {
    //                Id = 10
    //            });

    //        _unitOfWorkMock
    //            .Setup(x => x.Categories)
    //            .Returns(categoryRepositoryMock.Object);

    //        _productRepositoryMock
    //            .Setup(x => x.AddAsync(product))
    //            .Returns(Task.CompletedTask);

    //        _unitOfWorkMock
    //            .Setup(x => x.SaveChangesAsync())
    //            .ReturnsAsync(1);



    //        using MemoryStream stream = new MemoryStream(
    //    new byte[] { 1, 2, 3 });

    //        IFormFile image = new FormFile(
    //            stream,
    //            0,
    //            stream.Length,
    //            "Images",
    //            "laptop.jpg");

    //        List<IFormFile> images = new List<IFormFile>
    //{
    //    image
    //};
    //        _imageStorageMock
    //    .Setup(x => x.UploadAsync(
    //        It.IsAny<IFormFile>(),
    //        It.IsAny<string>()))
    //    .ReturnsAsync(
    //        new StoredImage(
    //            "https://example.com/laptop.jpg",
    //            "test/laptop"));

    //        Product? result =
    //            await _service.CreateAsync(product, images);


    //        Assert.Equal(product, result);

    //        _productRepositoryMock.Verify(
    //            x => x.AddAsync(product),
    //            Times.Once);

    //        _unitOfWorkMock.Verify(
    //            x => x.SaveChangesAsync(),
    //            Times.Once);
    //    }
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

        Mock<ICategoryRepository> categoryRepositoryMock =
            new Mock<ICategoryRepository>();

        categoryRepositoryMock
            .Setup(x => x.GetByIdAsync(10))
            .ReturnsAsync(new Category
            {
                Id = 10
            });

        _unitOfWorkMock
            .Setup(x => x.Categories)
            .Returns(categoryRepositoryMock.Object);

        _productRepositoryMock
            .Setup(x => x.AddAsync(product))
            .Returns(Task.CompletedTask);

        _imageRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Image>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        using MemoryStream stream = new MemoryStream(
            new byte[] { 1, 2, 3 });

        IFormFile image = new FormFile(
            stream,
            0,
            stream.Length,
            "Images",
            "laptop.jpg");

        List<IFormFile> images = new List<IFormFile>
    {
        image
    };

        _imageStorageMock
            .Setup(x => x.UploadAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>()))
            .ReturnsAsync(
                new StoredImage(
                    "https://example.com/laptop.jpg",
                    "test/laptop"));

        Product? result =
            await _service.CreateAsync(product, images);

        Assert.NotNull(result);
        Assert.Equal(product, result);
        Assert.Single(result.Images);

        Image createdImage = result.Images[0];

        Assert.Equal(1, createdImage.ProductId);
        Assert.Equal(
            "https://example.com/laptop.jpg",
            createdImage.Url);
        Assert.Equal("test/laptop", createdImage.PublicId);
        Assert.True(createdImage.IsMain);
        Assert.Equal(0, createdImage.SortOrder);

        _productRepositoryMock.Verify(
            x => x.AddAsync(product),
            Times.Once);

        _imageRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Image>()),
            Times.Once);

        _imageStorageMock.Verify(
            x => x.UploadAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<string>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Exactly(2));
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
