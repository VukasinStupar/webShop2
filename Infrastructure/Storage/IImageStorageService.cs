namespace webShop2.Infrastructure.Storage
{
    public record StoredImage(string Url, string PublicId);

    public interface IImageStorageService
    {
        Task<StoredImage> UploadAsync(IFormFile file, string folder);
        Task<bool> DeleteAsync(string publicId);
    }
}
