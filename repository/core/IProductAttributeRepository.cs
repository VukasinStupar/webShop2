using webShop2.model;

namespace webShop2.repository.core
{
    public interface IProductAttributeRepository
    : IBaseRepository<ProductAttribute>
    {
        Task<List<ProductAttribute>> GetByCategoryAsync(int categoryId);
    }
}
