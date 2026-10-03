using webShop2.dto.category;
using webShop2.model;

namespace webShop2.mapper;

public class CategoryMapper
{
    public CategoryDto ToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            ParentId = category.ParentId
        };
    }

    public Category ToEntity(CreateCategoryDto dto)
    {
        return new Category
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };
    }

    public void UpdateEntity(Category category, UpdateCategoryDto dto)
    {
        category.Name = dto.Name;
        category.ParentId = dto.ParentId;
    }
}