using webShop2.dto.payment;
using webShop2.model;

namespace webShop2.services.core;

public interface IPaymentService
{
    Task<Payment?> GetByIdAsync(int id);
    Task<List<Payment>> GetAllAsync();
    Task<List<Payment>> GetByOrderAsync(int orderId);
    Task<List<Payment>> GetByStatusAsync(string status);

    Task<Payment> CreateAsync(Payment payment);
    Task<Payment?> UpdateAsync(int id, Payment payment);
    Task<bool> DeleteAsync(int id);
}