using webShop2.model;

namespace webShop2.repository.core
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<List<Payment>> GetByOrderAsync(int orderId);

        Task<List<Payment>> GetByStatusAsync(string status);
    }
}
