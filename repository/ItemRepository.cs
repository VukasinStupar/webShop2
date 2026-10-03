using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;

namespace webShop2.repository
{
    public class ItemRepository
     : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<Item>> GetByOrderAsync(int orderId)
        {
            return await _dbSet
                .Where(x => x.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<Item>> GetByProductAsync(int productId)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }
    }
}
