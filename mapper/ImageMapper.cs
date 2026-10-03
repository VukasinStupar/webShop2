using webShop2.dto.image;
using webShop2.model;

namespace webShop2.mapper;

public class ImageMapper
{
    public ImageDto ToDto(Image image)
    {
        return new ImageDto
        {
            Id = image.Id,
            ProductId = image.ProductId,
            Url = image.Url,
            IsMain = image.IsMain,
            SortOrder = image.SortOrder
        };
    }

    public Image ToEntity(CreateImageDto dto)
    {
        return new Image
        {
            ProductId = dto.ProductId,
            Url = dto.Url,
            IsMain = dto.IsMain,
            SortOrder = dto.SortOrder
        };
    }

    public void UpdateEntity(Image image, UpdateImageDto dto)
    {
        image.Url = dto.Url;
        image.IsMain = dto.IsMain;
        image.SortOrder = dto.SortOrder;
    }
}