using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class OrderServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly OrderService _service;

public OrderServiceUnitTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _orderRepositoryMock = new Mock<IOrderRepository>();

        _unitOfWorkMock
            .Setup(x => x.Orders)
            .Returns(_orderRepositoryMock.Object);

        _service = new OrderService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOrder_WhenOrderExists()
    {
        Order order = new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Pending"
        };

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(order);

        Order? result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(order, result);

        _orderRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenOrderDoesNotExist()
    {
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Order?)null);

        Order? result = await _service.GetByIdAsync(1);

        Assert.Null(result);

        _orderRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllOrders()
    {
        List<Order> orders =
        [
            new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Pending"
        },
        new Order
        {
            Id = 2,
            Number = "ORD-002",
            Status = "Completed"
        }
        ];

        _orderRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(orders);

        List<Order> result = await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(orders, result);

        _orderRepositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByNumberAsync_ReturnsOrder_WhenOrderExists()
    {
        Order order = new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Pending"
        };

        _orderRepositoryMock
            .Setup(x => x.GetByNumberAsync("ORD-001"))
            .ReturnsAsync(order);

        Order? result =
            await _service.GetByNumberAsync("ORD-001");

        Assert.NotNull(result);
        Assert.Equal(order, result);

        _orderRepositoryMock.Verify(
            x => x.GetByNumberAsync("ORD-001"),
            Times.Once);
    }

    [Fact]
    public async Task GetByStatusAsync_ReturnsOrdersWithStatus()
    {
        List<Order> orders =
        [
            new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Pending"
        },
        new Order
        {
            Id = 2,
            Number = "ORD-002",
            Status = "Pending"
        }
        ];

        _orderRepositoryMock
            .Setup(x => x.GetByStatusAsync("Pending"))
            .ReturnsAsync(orders);

        List<Order> result =
            await _service.GetByStatusAsync("Pending");

        Assert.Equal(2, result.Count);
        Assert.Equal(orders, result);

        _orderRepositoryMock.Verify(
            x => x.GetByStatusAsync("Pending"),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_AddsOrderAndSavesChanges()
    {
        Order order = new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Pending"
        };

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Order result =
            await _service.CreateAsync(order);

        Assert.Equal(order, result);

        _orderRepositoryMock.Verify(
            x => x.AddAsync(order),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesOrder_WhenOrderExists()
    {
        Order existingOrder = new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Pending"
        };

        Order updateOrder = new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Completed"
        };

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingOrder);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Order? result =
            await _service.UpdateAsync(1, updateOrder);

        Assert.NotNull(result);
        Assert.Equal("Completed", result.Status);

        _orderRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _orderRepositoryMock.Verify(
            x => x.Update(existingOrder),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenOrderDoesNotExist()
    {
        Order updateOrder = new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Completed"
        };

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Order?)null);

        Order? result =
            await _service.UpdateAsync(1, updateOrder);

        Assert.Null(result);

        _orderRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _orderRepositoryMock.Verify(
            x => x.Update(It.IsAny<Order>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesOrder_WhenOrderExists()
    {
        Order order = new Order
        {
            Id = 1,
            Number = "ORD-001",
            Status = "Pending"
        };

        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(order);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        bool result =
            await _service.DeleteAsync(1);

        Assert.True(result);

        _orderRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _orderRepositoryMock.Verify(
            x => x.Delete(order),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenOrderDoesNotExist()
    {
        _orderRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Order?)null);

        bool result =
            await _service.DeleteAsync(1);

        Assert.False(result);

        _orderRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _orderRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Order>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

}
