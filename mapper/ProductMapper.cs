using webShop2.dto.productDto;
using webShop2.dto.ProductDto;
using webShop2.model;

namespace webShop2.mapper;

public class ProductMapper
{
    public ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            Status = product.Status,
            CategoryId = product.CategoryId
        };

    }
    public Product ToEntity(CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Quantity = dto.Quantity,
            CategoryId = dto.CategoryId
        };
    }

    public void UpdateEntity(Product product, UpdateProductDto dto)
    {
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Quantity = dto.Quantity;
        product.Status = dto.Status;
        product.CategoryId = dto.CategoryId;
    }


}