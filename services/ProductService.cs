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
        Product? product = await _unitOfWork.Products.GetByIdAsync(id);

        if (product == null)
        {
            return null;
        }

        return product;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        List<Product> products = await _unitOfWork.Products.GetAllAsync();

        return products.ToList();
    }

    public async Task<Product> CreateAsync(Product createProduct)
    {

        await _unitOfWork.Products.AddAsync(createProduct);
        await _unitOfWork.SaveChangesAsync();

        return createProduct;
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