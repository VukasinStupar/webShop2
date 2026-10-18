using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;
namespace webShop2.repository
{
    public class ProductRepository
    : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetActiveProductsAsync()
        {
            return await _dbSet
                .Where(x => x.Status == "Active")
                .ToListAsync();
        }

        public async Task<List<Product>> SearchByNameAsync(string name)
        {
            return await _dbSet
                .Where(x => x.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

       
        public async Task<Product?> GetByIdWithImagesAsync(int id)
        {
            return await _dbSet
                .Include(x => x.Images.OrderBy(i => i.SortOrder))
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Product>> GetAllWithImagesAsync()
        {
            return await _dbSet
                .Include(x => x.Images.OrderBy(i => i.SortOrder))
                .ToListAsync();
        }
    }
}
