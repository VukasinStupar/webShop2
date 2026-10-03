using webShop2.dto.item;
using webShop2.model;

namespace webShop2.services.core;

public interface IItemService
{
    Task<Item?> GetByIdAsync(int id);
    Task<List<Item>> GetAllAsync();
    Task<List<Item>> GetByOrderAsync(int orderId);
    Task<List<Item>> GetByProductAsync(int productId);

    Task<Item> CreateAsync(Item itemCreate);
    Task<Item?> UpdateAsync(int id, Item updateItem);
    Task<bool> DeleteAsync(int id);
}