using webShop2.dto.productAttribute;
using webShop2.model;

namespace webShop2.mapper;

public class ProductAttributeMapper
{
    public ProductAttributeDto ToDto(ProductAttribute attribute)
    {
        return new ProductAttributeDto
        {
            Id = attribute.Id,
            Name = attribute.Name,
            CategoryId = attribute.CategoryId,
            Type = attribute.Type
        };
    }

    public ProductAttribute ToEntity(CreateProductAttributeDto dto)
    {
        return new ProductAttribute
        {
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            Type = dto.Type
        };
    }

    public void UpdateEntity(
        ProductAttribute attribute,
        UpdateProductAttributeDto dto)
    {
        attribute.Name = dto.Name;
        attribute.CategoryId = dto.CategoryId;
        attribute.Type = dto.Type;
    }
}