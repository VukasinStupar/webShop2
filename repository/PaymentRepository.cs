using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;
namespace webShop2.repository
{
    public class PaymentRepository
    : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<Payment>> GetByOrderAsync(int orderId)
        {
            return await _dbSet
                .Where(x => x.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(x => x.Status == status)
                .ToListAsync();
        }
    }
}
