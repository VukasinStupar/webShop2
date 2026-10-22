using Microsoft.AspNetCore.Http;
using webShop2.model;

namespace webShop2.services.core;

public interface IProductService
{
    Task<Product?> GetByIdAsync(int id);

    Task<List<Product>> GetAllAsync();

    Task<Product?> CreateAsync(
        Product product,
        List<IFormFile> images);

    Task<Product?> UpdateAsync(
        int id,
        Product updateProduct);

    Task<bool> DeleteAsync(int id);
}