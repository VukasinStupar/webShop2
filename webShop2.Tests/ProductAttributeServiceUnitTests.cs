using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class ProductAttributeServiceUnitTests
{
    private readonly Mock<IProductAttributeRepository> _productAttributeRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ProductAttributeService _service;

    public ProductAttributeServiceUnitTests()
    {
        _productAttributeRepositoryMock =
            new Mock<IProductAttributeRepository>();

        _unitOfWorkMock =
            new Mock<IUnitOfWork>();

        _unitOfWorkMock
            .Setup(x => x.ProductAttributes)
            .Returns(_productAttributeRepositoryMock.Object);

        _service =
            new ProductAttributeService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsAttribute_WhenAttributeExists()
    {
        ProductAttribute attribute = new ProductAttribute
        {
            Id = 1,
            Name = "Color",
            Type = AttributeType.Text
        };

        _productAttributeRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(attribute);

        ProductAttribute? result =
            await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(attribute, result);

        _productAttributeRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenAttributeDoesNotExist()
    {
        _productAttributeRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((ProductAttribute?)null);

        ProductAttribute? result =
            await _service.GetByIdAsync(1);

        Assert.Null(result);

        _productAttributeRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllAttributes()
    {
        List<ProductAttribute> attributes =
        [
            new ProductAttribute
            {
                Id = 1,
                Name = "Color",
                Type = AttributeType.Text
            },
            new ProductAttribute
            {
                Id = 2,
                Name = "Size",
                Type = AttributeType.Text
            }
        ];

        _productAttributeRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(attributes);

        List<ProductAttribute> result =
            await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(attributes, result);

        _productAttributeRepositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByCategoryAsync_ReturnsAttributesForCategory()
    {
        List<ProductAttribute> attributes =
        [
            new ProductAttribute
            {
                Id = 1,
                CategoryId = 10,
                Name = "Color",
                Type = AttributeType.Text
            },
            new ProductAttribute
            {
                Id = 2,
                CategoryId = 10,
                Name = "Size",
                Type = AttributeType.Text
            }
        ];

        _productAttributeRepositoryMock
            .Setup(x => x.GetByCategoryAsync(10))
            .ReturnsAsync(attributes);

        List<ProductAttribute> result =
            await _service.GetByCategoryAsync(10);

        Assert.Equal(2, result.Count);

        Assert.All(
            result,
            attribute => Assert.Equal(10, attribute.CategoryId));

        _productAttributeRepositoryMock.Verify(
            x => x.GetByCategoryAsync(10),
            Times.Once);
    }

    [Fact]
    public async Task GetByCategoryAsync_ReturnsEmptyList_WhenCategoryHasNoAttributes()
    {
        List<ProductAttribute> attributes =
        [];

        _productAttributeRepositoryMock
            .Setup(x => x.GetByCategoryAsync(10))
            .ReturnsAsync(attributes);

        List<ProductAttribute> result =
            await _service.GetByCategoryAsync(10);

        Assert.Empty(result);

        _productAttributeRepositoryMock.Verify(
            x => x.GetByCategoryAsync(10),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_AddsAttributeAndSavesChanges()
    {
        ProductAttribute attribute = new ProductAttribute
        {
            Id = 1,
            CategoryId = 10,
            Name = "Color",
            Type = AttributeType.Text
        };

        _productAttributeRepositoryMock
            .Setup(x => x.AddAsync(attribute))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        ProductAttribute result =
            await _service.CreateAsync(attribute);

        Assert.Equal(attribute, result);

        _productAttributeRepositoryMock.Verify(
            x => x.AddAsync(attribute),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAttribute_WhenAttributeExists()
    {
        ProductAttribute existingAttribute = new ProductAttribute
        {
            Id = 1,
            CategoryId = 10,
            Name = "Color",
            Type = AttributeType.Text
        };

        ProductAttribute updateAttribute = new ProductAttribute
        {
            Id = 1,
            CategoryId = 10,
            Name = "Brand",
            Type = AttributeType.Text
        };

        _productAttributeRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingAttribute);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        ProductAttribute? result =
            await _service.UpdateAsync(1, updateAttribute);

        Assert.NotNull(result);
        Assert.Equal("Brand", result.Name);
        Assert.Equal(AttributeType.Text, result.Type);

        _productAttributeRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productAttributeRepositoryMock.Verify(
            x => x.Update(existingAttribute),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenAttributeDoesNotExist()
    {
        ProductAttribute updateAttribute = new ProductAttribute
        {
            Id = 1,
            Name = "Brand",
            Type = AttributeType.Text
        };

        _productAttributeRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((ProductAttribute?)null);

        ProductAttribute? result =
            await _service.UpdateAsync(1, updateAttribute);

        Assert.Null(result);

        _productAttributeRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productAttributeRepositoryMock.Verify(
            x => x.Update(It.IsAny<ProductAttribute>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesAttribute_WhenAttributeExists()
    {
        ProductAttribute attribute = new ProductAttribute
        {
            Id = 1,
            CategoryId = 10,
            Name = "Color",
            Type = AttributeType.Text
        };

        _productAttributeRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(attribute);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        bool result =
            await _service.DeleteAsync(1);

        Assert.True(result);

        _productAttributeRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productAttributeRepositoryMock.Verify(
            x => x.Delete(attribute),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenAttributeDoesNotExist()
    {
        _productAttributeRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((ProductAttribute?)null);

        bool result =
            await _service.DeleteAsync(1);

        Assert.False(result);

        _productAttributeRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _productAttributeRepositoryMock.Verify(
            x => x.Delete(It.IsAny<ProductAttribute>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }
}