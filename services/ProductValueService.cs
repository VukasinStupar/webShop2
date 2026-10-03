using webShop2.dto.productValue;
using webShop2.mapper;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services;

public class ProductValueService : IProductValueService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductValueService(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductValue?> GetByIdAsync(int id)
    {
        ProductValue? value =
            await _unitOfWork.ProductValues.GetByIdAsync(id);

        if (value == null)
        {
            return null;
        }

        return value;
    }

    public async Task<List<ProductValue>> GetAllAsync()
    {
        List<ProductValue> values =
            await _unitOfWork.ProductValues.GetAllAsync();

        return values.ToList();
    }

    public async Task<List<ProductValue>> GetByProductAsync(
        int productId)
    {
        List<ProductValue> values =
            await _unitOfWork.ProductValues.GetByProductAsync(productId);

        return values.ToList();
    }

    public async Task<List<ProductValue>> GetByAttributeAsync(
        int productAttributeId)
    {
        List<ProductValue> values =
            await _unitOfWork.ProductValues
                .GetByAttributeAsync(productAttributeId);

        return values.ToList();
    }

    public async Task<ProductValue> CreateAsync(
        ProductValue createProductValue)
    {
        await _unitOfWork.ProductValues.AddAsync(createProductValue);
        await _unitOfWork.SaveChangesAsync();

        return createProductValue;
    }

    public async Task<ProductValue?> UpdateAsync(
        int id,
        ProductValue updateProductValue)
    {
        ProductValue? value =
            await _unitOfWork.ProductValues.GetByIdAsync(id);

        if (value == null)
        {
            return null;
        }

        _unitOfWork.ProductValues.Update(value);
        await _unitOfWork.SaveChangesAsync();

        return value;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ProductValue? value =
            await _unitOfWork.ProductValues.GetByIdAsync(id);

        if (value == null)
        {
            return false;
        }

        _unitOfWork.ProductValues.Delete(value);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}