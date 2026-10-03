using webShop2.dto.order;
using webShop2.model;

namespace webShop2.mapper;

public class OrderMapper
{
    public OrderDto ToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            Number = order.Number,
            CustomerName = order.CustomerName,
            CustomerEmail = order.CustomerEmail,
            CustomerPhone = order.CustomerPhone,
            Address = order.Address,
            City = order.City,
            PostalCode = order.PostalCode,
            Total = order.Total,
            Status = order.Status
        };
    }

    public Order ToEntity(CreateOrderDto dto)
    {
        return new Order
        {
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            CustomerPhone = dto.CustomerPhone,
            Address = dto.Address,
            City = dto.City,
            PostalCode = dto.PostalCode
        };
    }

    public void UpdateEntity(Order order, UpdateOrderDto dto)
    {
        order.CustomerName = dto.CustomerName;
        order.CustomerEmail = dto.CustomerEmail;
        order.CustomerPhone = dto.CustomerPhone;
        order.Address = dto.Address;
        order.City = dto.City;
        order.PostalCode = dto.PostalCode;
        order.Status = dto.Status;
    }
}