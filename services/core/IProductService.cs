using webShop2.dto.productDto;
using webShop2.dto.ProductDto;
using webShop2.model;

namespace webShop2.services.core;

public interface IProductService
{
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetAllAsync();

    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(
        int id,
        Product product);

    Task<bool> DeleteAsync(int id);
}