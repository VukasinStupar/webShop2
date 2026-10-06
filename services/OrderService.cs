using webShop2.dto.order;
using webShop2.mapper;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(
        IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        Order? order = await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null)
        {
            return null;
        }

        return order;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        List<Order> orders = await _unitOfWork.Orders.GetAllAsync();

        return orders.ToList();
    }

    public async Task<Order?> GetByNumberAsync(string number)
    {
        Order? order =
            await _unitOfWork.Orders.GetByNumberAsync(number);

        if (order == null)
        {
            return null;
        }

        return order;
    }

    public async Task<List<Order>> GetByStatusAsync(string status)
    {
        List<Order> orders =
            await _unitOfWork.Orders.GetByStatusAsync(status);

        return orders.ToList();
    }

    public async Task<Order> CreateAsync(Order order)
    {

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> UpdateAsync(
    int id,
    Order orderUpdate)
    {
        Order? order =
            await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null)
        {
            return null;
        }

        order.Number = orderUpdate.Number;
        order.Status = orderUpdate.Status;

        _unitOfWork.Orders.Update(order);
        await _unitOfWork.SaveChangesAsync();

        return order;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Order? order =
            await _unitOfWork.Orders.GetByIdAsync(id);

        if (order == null)
        {
            return false;
        }

        _unitOfWork.Orders.Delete(order);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}