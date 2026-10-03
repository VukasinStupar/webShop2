using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;
namespace webShop2.repository
{
    public class OrderRepository
    : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<Order?> GetByNumberAsync(string number)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Number == number);
        }

        public async Task<List<Order>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(x => x.Status == status)
                .ToListAsync();
        }
    }
}
