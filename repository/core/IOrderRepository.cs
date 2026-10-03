using webShop2.model;

namespace webShop2.repository.core
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetByNumberAsync(string number);

        Task<List<Order>> GetByStatusAsync(string status);
    }
}
