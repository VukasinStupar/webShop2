namespace webShop2.services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using webShop2.dto.productDto;
    using webShop2.Infrastructure.Storage;
    using webShop2.model;
    using webShop2.repository.core;
    using webShop2.services.core;


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
            product.Quantity = updateProduct.Quantity;
            product.Status = updateProduct.Status;
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

        public async Task<Product?> CreateAsync(
     Product product,
     List<IFormFile> images)
        {
            // 1. Provera da li kategorija postoji
            webShop2.model.Category? category =
                await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

            if (category == null)
            {
                return null;
            }

            // 2. Provera da li su slike prosleđene
            if (images == null || images.Count == 0)
            {
                throw new ArgumentException(
                    "Morate dodati najmanje jednu sliku proizvoda.");
            }

            // 3. Validacija svih slika pre upload-a
            List<string> allowedExtensions =
                new List<string> { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (IFormFile image in images)
            {
                if (image == null || image.Length == 0)
                {
                    throw new ArgumentException(
                        "Jedna od prosleđenih slika je prazna.");
                }

                if (image.Length > 5 * 1024 * 1024)
                {
                    throw new ArgumentException(
                        "Svaka slika može imati najviše 5 MB.");
                }

                string extension =
                    Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    throw new ArgumentException(
                        "Dozvoljene su slike JPG, JPEG, PNG i WEBP formata.");
                }
            }

            // Evidencija slika uploadovanih na Cloudinary
            List<string> uploadedPublicIds = new List<string>();

            try
            {
                // 4. Dodavanje proizvoda u bazu da bi dobio ID
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                // Osiguraj da kolekcija postoji
                if (product.Images == null)
                {
                    product.Images = new List<webShop2.model.Image>();
                }

                // 5. Upload slika i dodavanje Image entiteta
                for (int i = 0; i < images.Count; i++)
                {
                    StoredImage storedImage = await _storage.UploadAsync(
                        images[i],
                        $"webshop/products/{product.Id}");

                    uploadedPublicIds.Add(storedImage.PublicId);

                    webShop2.model.Image productImage =
                        new webShop2.model.Image
                        {
                            ProductId = product.Id,
                            Url = storedImage.Url,
                            PublicId = storedImage.PublicId,
                            IsMain = i == 0,
                            SortOrder = i
                        };

                    // Dodavanje slike u kolekciju proizvoda
                    product.Images.Add(productImage);

                    // Dodavanje slike u repozitorijum
                    await _unitOfWork.Images.AddAsync(productImage);
                }

                // 6. Čuvanje slika u bazi
                await _unitOfWork.SaveChangesAsync();

                // 7. Vraćanje proizvoda zajedno sa slikama
                return product;
            }
            catch
            {
                // 8. Pokušaj brisanja uploadovanih slika sa Cloudinary-ja
                foreach (string publicId in uploadedPublicIds)
                {
                    try
                    {
                        await _storage.DeleteAsync(publicId);
                    }
                    catch
                    {
                        // Nastavi čišćenje i ako brisanje pojedinačne slike ne uspe.
                    }
                }

                // 9. Pokušaj uklanjanja proizvoda iz baze
                try
                {
                    if (product.Id > 0)
                    {
                        _unitOfWork.Products.Delete(product);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
                catch
                {
                    // Ne prikrivaj originalnu grešku.
                }

                throw;
            }
        }

    }
}