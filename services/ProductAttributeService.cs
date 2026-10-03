using webShop2.dto.productAttribute;
using webShop2.mapper;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services;

public class ProductAttributeService : IProductAttributeService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductAttributeService(
        IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductAttribute?> GetByIdAsync(int id)
    {
        ProductAttribute? attribute =
            await _unitOfWork.ProductAttributes.GetByIdAsync(id);

        if (attribute == null)
        {
            return null;
        }

        return attribute;
    }

    public async Task<List<ProductAttribute>> GetAllAsync()
    {
        List<ProductAttribute> attributes =
            await _unitOfWork.ProductAttributes.GetAllAsync();

        return attributes.ToList();
    }

    public async Task<List<ProductAttribute>> GetByCategoryAsync(
        int categoryId)
    {
        List<ProductAttribute> attributes =
            await _unitOfWork.ProductAttributes
                .GetByCategoryAsync(categoryId);

        return attributes.ToList();
    }

    public async Task<ProductAttribute> CreateAsync(
    ProductAttribute productAttribute)
    {
        await _unitOfWork.ProductAttributes.AddAsync(productAttribute);
        await _unitOfWork.SaveChangesAsync();

        return productAttribute;
    }
    public async Task<ProductAttribute?> UpdateAsync(
     int id,
     ProductAttribute updateProductAttribute)
    {
        ProductAttribute? attribute =
            await _unitOfWork.ProductAttributes.GetByIdAsync(id);

        if (attribute == null)
        {
            return null;
        }

        attribute.Name = updateProductAttribute.Name;
        attribute.Type = updateProductAttribute.Type;

        _unitOfWork.ProductAttributes.Update(attribute);
        await _unitOfWork.SaveChangesAsync();

        return attribute;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ProductAttribute? attribute =
            await _unitOfWork.ProductAttributes.GetByIdAsync(id);

        if (attribute == null)
        {
            return false;
        }

        _unitOfWork.ProductAttributes.Delete(attribute);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}