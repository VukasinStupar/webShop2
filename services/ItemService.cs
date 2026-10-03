using webShop2.dto.item;
using webShop2.mapper;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services;

public class ItemService : IItemService
{
    private readonly IUnitOfWork _unitOfWork;

    public ItemService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Item?> GetByIdAsync(int id)
    {
        Item? item = await _unitOfWork.Items.GetByIdAsync(id);

        if (item == null)
        {
            return null;
        }

        return item;
    }

    public async Task<List<Item>> GetAllAsync()
    {
        List<Item> items = await _unitOfWork.Items.GetAllAsync();
        return items;

    }

    public async Task<List<Item>> GetByOrderAsync(int orderId)
    {
        List<Item> items =
            await _unitOfWork.Items.GetByOrderAsync(orderId);

        return items.ToList();
    }

    public async Task<List<Item>> GetByProductAsync(int productId)
    {
        List<Item> items =
            await _unitOfWork.Items.GetByProductAsync(productId);

        return items.ToList();
    }

    public async Task<Item> CreateAsync(Item item)
    {

        await _unitOfWork.Items.AddAsync(item);
        await _unitOfWork.SaveChangesAsync();

        return item;
    }

    public async Task<Item?> UpdateAsync(
      int id,
      Item itemUpdate)
    {
        Item? item =
            await _unitOfWork.Items.GetByIdAsync(id);

        if (item == null)
        {
            return null;
        }

        item.Quantity = itemUpdate.Quantity;
        item.Price = itemUpdate.Price;

        _unitOfWork.Items.Update(item);
        await _unitOfWork.SaveChangesAsync();

        return item;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Item? item =
            await _unitOfWork.Items.GetByIdAsync(id);

        if (item == null)
        {
            return false;
        }

        _unitOfWork.Items.Delete(item);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

}
