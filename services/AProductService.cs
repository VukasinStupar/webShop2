using webShop2.Infrastructure.Storage;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;
using Microsoft.AspNetCore.Http;


public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IImageStorageService _storage;

    public ProductService(IUnitOfWork unitOfWork, IImageStorageService storage)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        //Product? product = await _unitOfWork.Products.GetByIdAsync(id);
        Product? product = await _unitOfWork.Products.GetByIdWithImagesAsync(id);


        if (product == null)
        {
            return null;
        }

        return product;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        //List<Product> products = await _unitOfWork.Products.GetAllAsync();
        List<Product> products = await _unitOfWork.Products.GetAllWithImagesAsync();

        Console.WriteLine("service Count: " + products.Count);
        return products.ToList();
    }



    public async Task<Product?> CreateAsync(
        Product product,
        List<IFormFile> images)
    {
        Category? category =
            await _unitOfWork.Categories.GetByIdAsync(
                product.CategoryId);

        if (category == null)
        {
            return null;
        }

        if (images == null || images.Count == 0)
        {
            throw new ArgumentException(
                "Morate dodati najmanje jednu sliku.");
        }

        foreach (IFormFile image in images)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException(
                    "Jedna od izabranih slika je prazna.");
            }

            if (image.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException(
                    "Slika ne sme biti veća od 5 MB.");
            }

            string extension =
                Path.GetExtension(image.FileName).ToLowerInvariant();

            if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png" &&
                extension != ".webp")
            {
                throw new ArgumentException(
                    "Dozvoljeni formati su JPG, PNG i WebP.");
            }
        }

        List<string> uploadedPublicIds = new List<string>();

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        try
        {
            for (int i = 0; i < images.Count; i++)
            {
                StoredImage stored = await _storage.UploadAsync(
                    images[i],
                    $"webshop/products/{product.Id}");

                uploadedPublicIds.Add(stored.PublicId);

                Image image = new Image
                {
                    ProductId = product.Id,
                    Product = product,
                    Url = stored.Url,
                    PublicId = stored.PublicId,
                    IsMain = i == 0,
                    SortOrder = i
                };

                await _unitOfWork.Images.AddAsync(image);

                product.Images.Add(image);
            }

            await _unitOfWork.SaveChangesAsync();

            return product;
        }
        catch
        {
            foreach (string publicId in uploadedPublicIds)
            {
                try
                {
                    await _storage.DeleteAsync(publicId);
                }
                catch
                {
                    // Ne prekidaj čišćenje ostalih uploadovanih slika.
                }
            }

            foreach (Image image in product.Images)
            {
                _unitOfWork.Images.Delete(image);
            }

            _unitOfWork.Products.Delete(product);

            await _unitOfWork.SaveChangesAsync();

            throw;
        }
    }


    public async Task<Product?> UpdateAsync(
    int id,
    Product updateProduct)
    {
        Product? product =
            await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return null;
        }

        product.Name = updateProduct.Name;
        product.Description = updateProduct.Description;
        product.Price = updateProduct.Price;
        product.CategoryId = updateProduct.CategoryId;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return false;
        }

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

   
}

