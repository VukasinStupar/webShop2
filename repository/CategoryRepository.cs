using System;
using System.Collections.Generic;
using System.Text;
using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;

namespace webShop2.repository
{
    public class CategoryRepository
    : BaseRepository<Category>, ICategoryRepository
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
    }
}
