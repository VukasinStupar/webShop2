using Moq;
using Xunit;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services;

namespace webShop2.Tests;

public class PaymentServiceUnitTests
{
    private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly PaymentService _service;

    public PaymentServiceUnitTests()
    {
        _paymentRepositoryMock =
            new Mock<IPaymentRepository>();

        _unitOfWorkMock =
            new Mock<IUnitOfWork>();

        _unitOfWorkMock
            .Setup(x => x.Payments)
            .Returns(_paymentRepositoryMock.Object);

        _service =
            new PaymentService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPayment_WhenPaymentExists()
    {
        Payment payment = new Payment
        {
            Id = 1,
            Status = "Paid"
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(payment);

        Payment? result =
            await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(payment, result);

        _paymentRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenPaymentDoesNotExist()
    {
        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Payment?)null);

        Payment? result =
            await _service.GetByIdAsync(1);

        Assert.Null(result);

        _paymentRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPayments()
    {
        List<Payment> payments = new List<Payment>
        {
            new Payment
            {
                Id = 1,
                Status = "Paid"
            },
            new Payment
            {
                Id = 2,
                Status = "Pending"
            }
        };

        _paymentRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(payments);

        List<Payment> result =
            await _service.GetAllAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(payments, result);

        _paymentRepositoryMock.Verify(
            x => x.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByOrderAsync_ReturnsPaymentsForOrder()
    {
        List<Payment> payments = new List<Payment>
        {
            new Payment
            {
                Id = 1,
                OrderId = 10,
                Status = "Paid"
            },
            new Payment
            {
                Id = 2,
                OrderId = 10,
                Status = "Pending"
            }
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByOrderAsync(10))
            .ReturnsAsync(payments);

        List<Payment> result =
            await _service.GetByOrderAsync(10);

        Assert.Equal(2, result.Count);
        Assert.All(
            result,
            payment => Assert.Equal(10, payment.OrderId));

        _paymentRepositoryMock.Verify(
            x => x.GetByOrderAsync(10),
            Times.Once);
    }

    [Fact]
    public async Task GetByStatusAsync_ReturnsPaymentsWithStatus()
    {
        List<Payment> payments = new List<Payment>
        {
            new Payment
            {
                Id = 1,
                Status = "Paid"
            },
            new Payment
            {
                Id = 2,
                Status = "Paid"
            }
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByStatusAsync("Paid"))
            .ReturnsAsync(payments);

        List<Payment> result =
            await _service.GetByStatusAsync("Paid");

        Assert.Equal(2, result.Count);
        Assert.All(
            result,
            payment => Assert.Equal("Paid", payment.Status));

        _paymentRepositoryMock.Verify(
            x => x.GetByStatusAsync("Paid"),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_AddsPaymentAndSavesChanges()
    {
        Payment payment = new Payment
        {
            Id = 1,
            OrderId = 10,
            Status = "Paid"
        };

        _paymentRepositoryMock
            .Setup(x => x.AddAsync(payment))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Payment result =
            await _service.CreateAsync(payment);

        Assert.Equal(payment, result);

        _paymentRepositoryMock.Verify(
            x => x.AddAsync(payment),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPayment_WhenPaymentExists()
    {
        Payment existingPayment = new Payment
        {
            Id = 1,
            OrderId = 10,
            Status = "Pending"
        };

        Payment updatePayment = new Payment
        {
            Id = 1,
            OrderId = 10,
            Status = "Paid"
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingPayment);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        Payment? result =
            await _service.UpdateAsync(1, updatePayment);

        Assert.NotNull(result);
        Assert.Equal("Paid", result.Status);

        _paymentRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Update(existingPayment),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenPaymentDoesNotExist()
    {
        Payment updatePayment = new Payment
        {
            Id = 1,
            Status = "Paid"
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Payment?)null);

        Payment? result =
            await _service.UpdateAsync(1, updatePayment);

        Assert.Null(result);

        _paymentRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Update(It.IsAny<Payment>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesPayment_WhenPaymentExists()
    {
        Payment payment = new Payment
        {
            Id = 1,
            OrderId = 10,
            Status = "Paid"
        };

        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(payment);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        bool result =
            await _service.DeleteAsync(1);

        Assert.True(result);

        _paymentRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Delete(payment),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenPaymentDoesNotExist()
    {
        _paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Payment?)null);

        bool result =
            await _service.DeleteAsync(1);

        Assert.False(result);

        _paymentRepositoryMock.Verify(
            x => x.GetByIdAsync(1),
            Times.Once);

        _paymentRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Payment>()),
            Times.Never);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }
}