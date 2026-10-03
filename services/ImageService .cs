using webShop2.dto.image;
using webShop2.mapper;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services;

public class ImageService : IImageService
{
    private readonly IUnitOfWork _unitOfWork;

    public ImageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

        _unitOfWork.Images.Update(existingImage);
        await _unitOfWork.SaveChangesAsync();

        return existingImage;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Image? image =
            await _unitOfWork.Images.GetByIdAsync(id);

        if (image == null)
        {
            return false;
        }

        _unitOfWork.Images.Delete(image);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}