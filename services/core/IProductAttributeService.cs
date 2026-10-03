using webShop2.dto.productAttribute;
using webShop2.model;

namespace webShop2.services.core;

public interface IProductAttributeService
{
    Task<ProductAttribute?> GetByIdAsync(int id);
    Task<List<ProductAttribute>> GetAllAsync();
    Task<List<ProductAttribute>> GetByCategoryAsync(int categoryId);

    Task<ProductAttribute> CreateAsync(ProductAttribute productAttribute);
    Task<ProductAttribute?> UpdateAsync(
        int id,
        ProductAttribute productAttribute);
    Task<bool> DeleteAsync(int id);
}