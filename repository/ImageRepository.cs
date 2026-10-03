using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;

namespace webShop2.repository

{
    public class ImageRepository
     : BaseRepository<Image>, IImageRepository
    {
        public ImageRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<Image>> GetByProductAsync(int productId)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        public async Task<Image?> GetMainImageAsync(int productId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x =>
                    x.ProductId == productId &&
                    x.IsMain);
        }
    }
}
