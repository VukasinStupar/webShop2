using webShop2.dto.item;
using webShop2.model;

namespace webShop2.mapper;

public class ItemMapper
{
    public ItemDto ToDto(Item item)
    {
        return new ItemDto
        {
            Id = item.Id,
            OrderId = item.OrderId,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            Price = item.Price
        };
    }

    public Item ToEntity(CreateItemDto dto)
    {
        return new Item
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };
    }

    public Item ToEntityX(ItemDto dto)
    {
        return new Item
        {
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity
        };
    }


    public void UpdateEntity(Item item, UpdateItemDto dto)
    {
        item.Quantity = dto.Quantity;
    }
}