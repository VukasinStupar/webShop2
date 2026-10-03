using webShop2.model;

namespace webShop2.repository.core;

public interface IImageRepository : IBaseRepository<Image>
{
    Task<List<Image>> GetByProductAsync(int productId);

    Task<Image?> GetMainImageAsync(int productId);
}