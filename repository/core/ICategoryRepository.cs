using System;
using System.Collections.Generic;
using System.Text;
using webShop2.model;
using webShop2.dto.category;

namespace webShop2.repository.core
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetRootCategoriesAsync();

        Task<List<Category>> GetChildrenAsync(int parentId);

        Task<List<CategoryStatsDto>> GetCategoryStatisticsAsync();
    }
}
