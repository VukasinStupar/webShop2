using webShop2.dto.image;
using webShop2.model;

using Microsoft.AspNetCore.Http;

namespace webShop2.services.core;

public interface IImageService
{
    Task<Image?> GetByIdAsync(int id);
    Task<List<Image>> GetAllAsync();
    Task<List<Image>> GetByProductAsync(int productId);
    Task<Image?> GetMainImageAsync(int productId);

    Task<Image> CreateAsync(Image image);
    Task<Image?> UpdateAsync(Image image);
    Task<bool> DeleteAsync(int id);

    Task<Image?> UploadAsync(int productId, IFormFile file, bool isMain);

    Task<Image?> SetMainAsync(int id);

}