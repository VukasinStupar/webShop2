using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;
namespace webShop2.repository
{
    public class ProductValueRepository
    : BaseRepository<ProductValue>,
      IProductValueRepository
    {
        public ProductValueRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<ProductValue>> GetByProductAsync(
            int productId)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        public async Task<List<ProductValue>> GetByAttributeAsync(
            int productAttributeId)
        {
            return await _dbSet
                .Where(x => x.ProductAttributeId == productAttributeId)
                .ToListAsync();
        }
    }
}
