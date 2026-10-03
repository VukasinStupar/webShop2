using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;

namespace webShop2.repository
{
    public class ProductAttributeRepository : BaseRepository<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributeRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<ProductAttribute>> GetByCategoryAsync(
            int categoryId)
        {
            return await _dbSet
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
