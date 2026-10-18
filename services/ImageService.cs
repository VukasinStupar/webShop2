using webShop2.dto.image;
using webShop2.Infrastructure.Storage;
using webShop2.mapper;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;
using Microsoft.AspNetCore.Http;

namespace webShop2.services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageStorageService _storage;

    public ImageService(IUnitOfWork unitOfWork, IImageStorageService storage)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
    }

    public async Task<Image?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Images.GetByIdAsync(id);
    }



    public async Task<List<Image>> GetAllAsync()
    {
        return await _unitOfWork.Images.GetAllAsync();
    }

    public async Task<List<Image>> GetByProductAsync(int productId)
    {
        return await _unitOfWork.Images.GetByProductAsync(productId);
    }

    public async Task<Image?> GetMainImageAsync(int productId)
    {
        return await _unitOfWork.Images.GetMainImageAsync(productId);
    }

    public async Task<Image> CreateAsync(Image image)
    {
        await _unitOfWork.Images.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();

        return image;
    }

    public async Task<Image?> UpdateAsync(Image image)
    {
        Image? existingImage =
            await _unitOfWork.Images.GetByIdAsync(image.Id);

        if (existingImage == null)
        {
            return null;
        }

        existingImage.Url = image.Url;
        existingImage.IsMain = image.IsMain;
        existingImage.SortOrder = image.SortOrder;

        _unitOfWork.Images.Update(existingImage);
        await _unitOfWork.SaveChangesAsync();

        return existingImage;
    }

    public async Task<Image?> SetMainAsync(int id)
    {
        Image? image = await _unitOfWork.Images.GetByIdAsync(id);

        if (image == null)
        {
            return null;
        }

        List<Image> productImages =
            await _unitOfWork.Images.GetByProductAsync(image.ProductId);

        foreach (Image item in productImages)
        {
            bool shouldBeMain = item.Id == id;

            if (item.IsMain != shouldBeMain)
            {
                item.IsMain = shouldBeMain;
                _unitOfWork.Images.Update(item);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return image;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Image? image =
            await _unitOfWork.Images.GetByIdAsync(id);

        if (image == null)
        {
            return false;
        }

        bool wasMain = image.IsMain;
        int productId = image.ProductId;

        // briši sa Cloudinary-ja (ako ima PublicId)
        if (!string.IsNullOrEmpty(image.PublicId))
        {
            await _storage.DeleteAsync(image.PublicId);
        }

        _unitOfWork.Images.Delete(image);

        // ako je obrisana glavna, prva preostala postaje glavna
        if (wasMain)
        {
            List<Image> remaining =
                (await _unitOfWork.Images.GetByProductAsync(productId))
                ?? new List<Image>();

            Image? next = remaining
                .Where(x => x.Id != id)
                .OrderBy(x => x.SortOrder)
                .FirstOrDefault();

            if (next != null)
            {
                next.IsMain = true;
                _unitOfWork.Images.Update(next);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<Image?> UploadAsync(int productId, IFormFile file, bool isMain)
    {
        Product? product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product == null)
        {
            return null;
        }

        StoredImage stored = await _storage.UploadAsync(file, $"webshop/products/{productId}");

        List<Image> existing = await _unitOfWork.Images.GetByProductAsync(productId);

        // prva slika proizvoda je uvek glavna
        if (existing.Count == 0)
        {
            isMain = true;
        }

        // ako je nova glavna, skini oznaku sa stare
        if (isMain)
        {
            foreach (Image old in existing.Where(x => x.IsMain))
            {
                old.IsMain = false;
                _unitOfWork.Images.Update(old);
            }
        }

        Image image = new Image
        {
            ProductId = productId,
            Url = stored.Url,
            PublicId = stored.PublicId,
            IsMain = isMain,
            SortOrder = existing.Count
        };

        await _unitOfWork.Images.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();

        return image;
    }
}