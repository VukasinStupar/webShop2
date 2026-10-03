using webShop2.model;

namespace webShop2.repository.core
{
    public interface IProductValueRepository
    : IBaseRepository<ProductValue>
    {
        Task<List<ProductValue>> GetByProductAsync(int productId);

        Task<List<ProductValue>> GetByAttributeAsync(
            int productAttributeId);
    }
}
