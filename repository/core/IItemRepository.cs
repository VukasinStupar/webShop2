using webShop2.model;

namespace webShop2.repository.core
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Task<List<Item>> GetByOrderAsync(int orderId);

        Task<List<Item>> GetByProductAsync(int productId);
    }
}
