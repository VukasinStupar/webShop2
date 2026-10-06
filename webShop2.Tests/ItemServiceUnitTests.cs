using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class ItemServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IItemRepository> _itemRepositoryMock;
    private readonly ItemService _service;

public ItemServiceUnitTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _itemRepositoryMock = new Mock<IItemRepository>();

        _unitOfWorkMock
            .Setup(x => x.Items)
            .Returns(_itemRepositoryMock.Object);

        _service = new ItemService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsItem_WhenItemExists()
    {
        Item item = new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 2,
            Price = 100
        };

        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(item);

        Item? result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(item, result);

        _itemRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenItemDoesNotExist()
    {
        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Item?)null);

        Item? result = await _service.GetByIdAsync(1);

        Assert.Null(result);

        _itemRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllItems()
    {
        List<Item> items =
        [
            new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 2,
            Price = 100
        },
        new Item
        {
            Id = 2,
            OrderId = 10,
            ProductId = 30,
            Quantity = 1,
            Price = 200
        }
        ];

        _itemRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(items);

        List<Item> result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(items, result);

        _itemRepositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByOrderAsync_ReturnsItemsForOrder()
    {
        List<Item> items =
        [
            new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 2,
            Price = 100
        },
        new Item
        {
            Id = 2,
            OrderId = 10,
            ProductId = 30,
            Quantity = 1,
            Price = 200
        }
        ];

        _itemRepositoryMock
            .Setup(x => x.GetByOrderAsync(10))
            .ReturnsAsync(items);

        List<Item> result =
            await _service.GetByOrderAsync(10);

        Assert.Equal(2, result.Count);
        Assert.Equal(items, result);

        _itemRepositoryMock.Verify(
            x => x.GetByOrderAsync(10),
            Times.Once);
    }

    [Fact]
    public async Task GetByProductAsync_ReturnsItemsForProduct()
    {
        List<Item> items =
        [
            new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 2,
            Price = 100
        },
        new Item
        {
            Id = 2,
            OrderId = 11,
            ProductId = 20,
            Quantity = 1,
            Price = 150
        }
        ];

        _itemRepositoryMock
            .Setup(x => x.GetByProductAsync(20))
            .ReturnsAsync(items);

        List<Item> result =
            await _service.GetByProductAsync(20);

        Assert.Equal(2, result.Count);
        Assert.Equal(items, result);

        _itemRepositoryMock.Verify(
            x => x.GetByProductAsync(20),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_AddsItemAndSavesChanges()
    {
        Item item = new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 2,
            Price = 100
        };

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Item result = await _service.CreateAsync(item);

        Assert.Equal(item, result);

        _itemRepositoryMock.Verify(
            x => x.AddAsync(item),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesItem_WhenItemExists()
    {
        Item existingItem = new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 2,
            Price = 100
        };

        Item updateItem = new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 5,
            Price = 150
        };

        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingItem);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Item? result =
            await _service.UpdateAsync(1, updateItem);

        Assert.NotNull(result);
        Assert.Equal(5, result.Quantity);
        Assert.Equal(150, result.Price);

        _itemRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _itemRepositoryMock.Verify(
            x => x.Update(existingItem),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenItemDoesNotExist()
    {
        Item updateItem = new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 5,
            Price = 150
        };

        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Item?)null);

        Item? result =
            await _service.UpdateAsync(1, updateItem);

        Assert.Null(result);

        _itemRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _itemRepositoryMock.Verify(
            x => x.Update(It.IsAny<Item>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesItem_WhenItemExists()
    {
        Item item = new Item
        {
            Id = 1,
            OrderId = 10,
            ProductId = 20,
            Quantity = 2,
            Price = 100
        };

        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(item);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        bool result = await _service.DeleteAsync(1);

        Assert.True(result);

        _itemRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _itemRepositoryMock.Verify(
            x => x.Delete(item),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenItemDoesNotExist()
    {
        _itemRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Item?)null);

        bool result = await _service.DeleteAsync(1);

        Assert.False(result);

        _itemRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _itemRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Item>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

}
