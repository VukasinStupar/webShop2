using webShop2.dto.order;
using webShop2.model;

namespace webShop2.services.core;

public interface IOrderService
{
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByNumberAsync(string number);
    Task<List<Order>> GetByStatusAsync(string status);

    Task<Order> CreateAsync(Order order);
    Task<Order?> UpdateAsync(int id, Order order);
    Task<bool> DeleteAsync(int id);
}