using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace webShop2.Infrastructure.Storage;
public class CloudinaryImageStorageService : IImageStorageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryImageStorageService(IOptions<CloudinarySettings> options)
    {
        CloudinarySettings s = options.Value;
        _cloudinary = new Cloudinary(new Account(s.CloudName, s.ApiKey, s.ApiSecret));
        _cloudinary.Api.Secure = true;
    }

    //public async Task<StoredImage> UploadAsync(IFormFile file, string folder)
    //{
    //    await using Stream stream = file.OpenReadStream();

    //    ImageUploadParams uploadParams = new ImageUploadParams
    //    {
    //        File = new FileDescription(file.FileName, stream),
    //        Folder = folder
    //    };

    //    ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams);

    //    if (result.Error != null)
    //    {
    //        throw new InvalidOperationException(result.Error.Message);
    //    }

    //    return new StoredImage(result.SecureUrl.ToString(), result.PublicId);
    //}
    public async Task<StoredImage> UploadAsync(IFormFile file, string folder)
    {
        try
        {
            Console.WriteLine($"CLOUDINARY upload start: {file.FileName}, {file.Length} bytes");

            await using Stream stream = file.OpenReadStream();

            ImageUploadParams uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder
            };

            ImageUploadResult result = await _cloudinary.UploadAsync(uploadParams);

            Console.WriteLine($"CLOUDINARY upload done. Error: {result.Error?.Message ?? "none"}");

            if (result.Error != null)
            {
                throw new InvalidOperationException(result.Error.Message);
            }

            return new StoredImage(result.SecureUrl.ToString(), result.PublicId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("CLOUDINARY GRESKA: " + ex);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string publicId)
    {
        DeletionResult result =
            await _cloudinary.DestroyAsync(new DeletionParams(publicId));

        return result.Result == "ok";
    }
}