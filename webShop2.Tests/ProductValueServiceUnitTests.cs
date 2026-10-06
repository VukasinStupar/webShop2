
using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class ProductValueServiceUnitTests
{
    private readonly Mock<IProductValueRepository> _productValueRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProductValueService _service;

    public ProductValueServiceUnitTests()
    {
        _productValueRepositoryMock =
            new Mock<IProductValueRepository>();

        _unitOfWorkMock =
            new Mock<IUnitOfWork>();

        _unitOfWorkMock
            .Setup(x => x.ProductValues)
            .Returns(_productValueRepositoryMock.Object);

        _service =
            new ProductValueService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsValue_WhenValueExists()
    {
        ProductValue value = new ProductValue
        {
            Id = 1
        };

        _productValueRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(value);

        ProductValue? result =
            await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(value, result);

        _productValueRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenValueDoesNotExist()
    {
        _productValueRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((ProductValue?)null);

        ProductValue? result =
            await _service.GetByIdAsync(1);

        Assert.Null(result);

        _productValueRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllValues()
    {
        List<ProductValue> values =
        [
            new ProductValue
            {
                Id = 1
            },
            new ProductValue
            {
                Id = 2
            }
        ];

        _productValueRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(values);

        List<ProductValue> result =
            await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(values, result);

        _productValueRepositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByProductAsync_ReturnsValuesForProduct()
    {
        List<ProductValue> values =
        [
            new ProductValue
            {
                Id = 1,
                ProductId = 10
            },
            new ProductValue
            {
                Id = 2,
                ProductId = 10
            }
        ];

        _productValueRepositoryMock
            .Setup(x => x.GetByProductAsync(10))
            .ReturnsAsync(values);

        List<ProductValue> result =
            await _service.GetByProductAsync(10);

        Assert.Equal(2, result.Count);

        Assert.All(
            result,
            value => Assert.Equal(10, value.ProductId));

        _productValueRepositoryMock.Verify(
            x => x.GetByProductAsync(10),
            Times.Once);
    }

    [Fact]
    public async Task GetByAttributeAsync_ReturnsValuesForAttribute()
    {
        List<ProductValue> values =
        [
            new ProductValue
            {
                Id = 1,
                ProductAttributeId = 20
            },
            new ProductValue
            {
                Id = 2,
                ProductAttributeId = 20
            }
        ];

        _productValueRepositoryMock
            .Setup(x => x.GetByAttributeAsync(20))
            .ReturnsAsync(values);

        List<ProductValue> result =
            await _service.GetByAttributeAsync(20);

        Assert.Equal(2, result.Count);

        Assert.All(
            result,
            value => Assert.Equal(20, value.ProductAttributeId));

        _productValueRepositoryMock.Verify(
            x => x.GetByAttributeAsync(20),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_AddsValueAndSavesChanges()
    {
        ProductValue value = new ProductValue
        {
            Id = 1,
            ProductId = 10,
            ProductAttributeId = 20
        };

        _productValueRepositoryMock
            .Setup(x => x.AddAsync(value))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        ProductValue result =
            await _service.CreateAsync(value);

        Assert.Equal(value, result);

        _productValueRepositoryMock.Verify(
            x => x.AddAsync(value),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesValue_WhenValueExists()
    {
        ProductValue existingValue = new ProductValue
        {
            Id = 1,
            ProductId = 10,
            ProductAttributeId = 20
        };

        ProductValue updateValue = new ProductValue
        {
            Id = 1,
            ProductId = 10,
            ProductAttributeId = 20
        };

        _productValueRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingValue);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        ProductValue? result =
            await _service.UpdateAsync(1, updateValue);

        Assert.NotNull(result);
        Assert.Equal(existingValue, result);

        _productValueRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productValueRepositoryMock.Verify(
            x => x.Update(existingValue),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenValueDoesNotExist()
    {
        ProductValue updateValue = new ProductValue
        {
            Id = 1,
            ProductId = 10,
            ProductAttributeId = 20
        };

        _productValueRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((ProductValue?)null);

        ProductValue? result =
            await _service.UpdateAsync(1, updateValue);

        Assert.Null(result);

        _productValueRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productValueRepositoryMock.Verify(
            x => x.Update(It.IsAny<ProductValue>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesValue_WhenValueExists()
    {
        ProductValue value = new ProductValue
        {
            Id = 1,
            ProductId = 10,
            ProductAttributeId = 20
        };

        _productValueRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(value);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        bool result =
            await _service.DeleteAsync(1);

        Assert.True(result);

        _productValueRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productValueRepositoryMock.Verify(
            x => x.Delete(value),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenValueDoesNotExist()
    {
        _productValueRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((ProductValue?)null);

        bool result =
            await _service.DeleteAsync(1);

        Assert.False(result);

        _productValueRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productValueRepositoryMock.Verify(
            x => x.Delete(It.IsAny<ProductValue>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }
}