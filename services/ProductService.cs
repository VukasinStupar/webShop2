using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

//namespace webShop2.services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
    //public async Task<Product?> GetByIdAsync(int id)
    //{
    //    return await _unitOfWork.Products.GetByIdWithImagesAsync(id);
    //}

    //public async Task<List<Product>> GetAllAsync()
    //{
    //    return await _unitOfWork.Products.GetAllWithImagesAsync();
    //}

    public async Task<Product?> CreateAsync(Product product)
    {
        
        Category? category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);

        if (category == null)
        {
            return null;
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return product;
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

