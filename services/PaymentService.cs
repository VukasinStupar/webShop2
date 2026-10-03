using webShop2.dto.payment;
using webShop2.mapper;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(
        IUnitOfWork unitOfWork
       )
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Payment?> GetByIdAsync(int id)
    {
        Payment? payment =
            await _unitOfWork.Payments.GetByIdAsync(id);

        if (payment == null)
        {
            return null;
        }

        return payment;
    }

    public async Task<List<Payment>> GetAllAsync()
    {
        List<Payment> payments =
            await _unitOfWork.Payments.GetAllAsync();

        return payments.ToList();
    }

    public async Task<List<Payment>> GetByOrderAsync(int orderId)
    {
        List<Payment> payments =
            await _unitOfWork.Payments.GetByOrderAsync(orderId);

        return payments.ToList();
    }

    public async Task<List<Payment>> GetByStatusAsync(string status)
    {
        List<Payment> payments =
            await _unitOfWork.Payments.GetByStatusAsync(status);

        return payments.ToList();
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {

        await _unitOfWork.Payments.AddAsync(payment);
        await _unitOfWork.SaveChangesAsync();

        return payment;
    }

    public async Task<Payment?> UpdateAsync(int id,
    Payment paymentUpdate)
    {
        Payment? payment =
            await _unitOfWork.Payments.GetByIdAsync(id);

        if (payment == null)
        {
            return null;
        }

        payment.Status = paymentUpdate.Status;

        _unitOfWork.Payments.Update(payment);
        await _unitOfWork.SaveChangesAsync();

        return payment;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Payment? payment =
            await _unitOfWork.Payments.GetByIdAsync(id);

        if (payment == null)
        {
            return false;
        }

        _unitOfWork.Payments.Delete(payment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}