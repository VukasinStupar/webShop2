using System.Threading.Tasks;
namespace webShop2.repository.core

{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IImageRepository Images { get; }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }
        IItemRepository Items { get; }
        IPaymentRepository Payments { get; }
        IProductAttributeRepository ProductAttributes { get; }
        IProductValueRepository ProductValues { get; }

        Task<int> SaveChangesAsync();
    }
}
