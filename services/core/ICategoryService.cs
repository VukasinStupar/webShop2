using webShop2.dto.category;
using webShop2.model;

namespace webShop2.services.core;

public interface ICategoryService
{
    Task<Category?> GetByIdAsync(int id);
    Task<List<Category>> GetAllAsync();
    Task<List<Category>> GetRootCategoriesAsync();
    Task<List<Category>> GetChildrenAsync(int parentId);

    Task<Category> CreateAsync(Category category);
    Task<Category?> UpdateAsync(int id, Category updateCategory);
    Task<bool> DeleteAsync(int id);
}