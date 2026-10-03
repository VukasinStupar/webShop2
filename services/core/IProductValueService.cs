using webShop2.dto.productValue;
using webShop2.model;

namespace webShop2.services.core;

public interface IProductValueService
{
    Task<ProductValue?> GetByIdAsync(int id);
    Task<List<ProductValue>> GetAllAsync();
    Task<List<ProductValue>> GetByProductAsync(int productId);
    Task<List<ProductValue>> GetByAttributeAsync(int productAttributeId);

    Task<ProductValue> CreateAsync(ProductValue productValue);
    Task<ProductValue?> UpdateAsync(
        int id,
        ProductValue productValue);
    Task<bool> DeleteAsync(int id);
}