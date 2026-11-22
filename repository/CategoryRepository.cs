
using Microsoft.EntityFrameworkCore;
using webShop2.dto.category;
using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;

namespace webShop2.repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            return await _dbSet
                .Where(x => x.ParentId == null)
                .ToListAsync();
        }

        public async Task<List<Category>> GetChildrenAsync(int parentId)
        {
            return await _dbSet
                .Where(x => x.ParentId == parentId)
                .ToListAsync();
        }


        public async Task<List<CategoryStatsDto>> GetCategoryStatisticsAsync()
        {
            return await (
                from product in _context.Products
                join category in _context.Categories
                    on product.CategoryId equals category.Id into categories
                from category in categories.DefaultIfEmpty()
                group product by new
                {
                    product.CategoryId,
                    Name = category != null
                        ? category.Name
                        : "Nepoznata kategorija"
                }
                into productGroup
                select new CategoryStatsDto
                {
                    CategoryId = productGroup.Key.CategoryId,
                    Name = productGroup.Key.Name,
                    Count = productGroup.Count(),
                    TotalPrice = productGroup.Sum(product => product.Price)
                }
            ).ToListAsync();
        }

    }
}
