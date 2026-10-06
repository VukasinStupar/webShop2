//using webShop2.dto.category;
//using webShop2.mapper;
//using webShop2.model;
//using webShop2.repository.core;
//using webShop2.services.core;

//namespace webShop2.services;

//public class CategoryService : ICategoryService
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public CategoryService(
//        IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<Category?> GetByIdAsync(int id)
//    {
//        Category? category = await _unitOfWork.Categories.GetByIdAsync(id);

//        if (category == null)
//        {
//            return null;
//        }

//        return category;
//    }

//    public async Task<List<Category>> GetAllAsync()
//    {
//        List<Category> categories = await _unitOfWork.Categories.GetAllAsync();

//        return categories.ToList();
//    }

//    public async Task<List<Category>> GetRootCategoriesAsync()
//    {
//        List<Category> categories =
//            await _unitOfWork.Categories.GetRootCategoriesAsync();

//        return categories.ToList();
//    }

//    public async Task<List<Category>> GetChildrenAsync(int parentId)
//    {
//        List<Category> categories =
//            await _unitOfWork.Categories.GetChildrenAsync(parentId);

//        return categories.ToList();
//    }

//    public async Task<Category> CreateAsync(Category category)
//    {

//        await _unitOfWork.Categories.AddAsync(category);
//        await _unitOfWork.SaveChangesAsync();

//        return category;
//    }

//    public async Task<Category?> UpdateAsync(
//        int id,
//        Category category)
//    {
//        Category? categori =
//            await _unitOfWork.Categories.GetByIdAsync(id);

//        if (categori == null)
//        {
//            return null;
//        }

//        _unitOfWork.Categories.Update(categori);
//        await _unitOfWork.SaveChangesAsync();

//        return categori;
//    }

//    public async Task<bool> DeleteAsync(int id)
//    {
//        Category? category =
//            await _unitOfWork.Categories.GetByIdAsync(id);

//        if (category == null)
//        {
//            return false;
//        }

//        _unitOfWork.Categories.Delete(category);
//        await _unitOfWork.SaveChangesAsync();

//        return true;
//    }
//}

using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Categories.GetByIdAsync(id);
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _unitOfWork.Categories.GetAllAsync();
    }

    public async Task<List<Category>> GetRootCategoriesAsync()
    {
        return await _unitOfWork.Categories.GetRootCategoriesAsync();
    }

    public async Task<List<Category>> GetChildrenAsync(int parentId)
    {
        return await _unitOfWork.Categories.GetChildrenAsync(parentId);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return category;
    }

    public async Task<Category?> UpdateAsync(
        int id,
        Category category)
    {
        Category? existingCategory =
            await _unitOfWork.Categories.GetByIdAsync(id);

        if (existingCategory == null)
        {
            return null;
        }

        existingCategory.Name = category.Name;
        existingCategory.ParentId = category.ParentId;

        _unitOfWork.Categories.Update(existingCategory);

        await _unitOfWork.SaveChangesAsync();

        return existingCategory;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Category? category =
            await _unitOfWork.Categories.GetByIdAsync(id);

        if (category == null)
        {
            return false;
        }

        _unitOfWork.Categories.Delete(category);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}