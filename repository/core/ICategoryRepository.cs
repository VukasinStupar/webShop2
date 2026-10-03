using System;
using System.Collections.Generic;
using System.Text;
using webShop2.model;

namespace webShop2.repository.core
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetRootCategoriesAsync();

        Task<List<Category>> GetChildrenAsync(int parentId);
    }
}
