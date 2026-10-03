using webShop2.model;

namespace webShop2.repository.core
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> GetByCategoryAsync(int categoryId);

        Task<List<Product>> GetActiveProductsAsync();

        Task<List<Product>> SearchByNameAsync(string name);
    }
}
