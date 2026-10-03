using webShop2.dto.productValue;
using webShop2.model;

namespace webShop2.mapper;

public class ProductValueMapper
{
    public ProductValueDto ToDto(ProductValue value)
    {
        return new ProductValueDto
        {
            Id = value.Id,
            ProductId = value.ProductId,
            ProductAttributeId = value.ProductAttributeId,
            Text = value.Text,
            Number = value.Number,
            Decimal = value.Decimal,
            Bool = value.Bool
        };
    }

    public ProductValue ToEntity(CreateProductValueDto dto)
    {
        return new ProductValue
        {
            ProductId = dto.ProductId,
            ProductAttributeId = dto.ProductAttributeId,
            Text = dto.Text,
            Number = dto.Number,
            Decimal = dto.Decimal,
            Bool = dto.Bool
        };
    }

    public void UpdateEntity(ProductValue value, UpdateProductValueDto dto)
    {
        value.Text = dto.Text;
        value.Number = dto.Number;
        value.Decimal = dto.Decimal;
        value.Bool = dto.Bool;
    }
}