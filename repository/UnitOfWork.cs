using webShop2.Infrastructure.Data;
using webShop2.repository.core;

namespace webShop2.repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IImageRepository Images { get; }
        public IUserRepository Users { get; }
        public IOrderRepository Orders { get; }
        public IItemRepository Items { get; }
        public IPaymentRepository Payments { get; }
        public IProductAttributeRepository ProductAttributes { get; }
        public IProductValueRepository ProductValues { get; }

        public UnitOfWork(
            AppDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IImageRepository imageRepository,
            IUserRepository userRepository,
            IOrderRepository orderRepository,
            IItemRepository itemRepository,
            IPaymentRepository paymentRepository,
            IProductAttributeRepository productAttributeRepository,
            IProductValueRepository productValueRepository)
        {
            _context = context;

            Products = productRepository;
            Categories = categoryRepository;
            Images = imageRepository;
            Users = userRepository;
            Orders = orderRepository;
            Items = itemRepository;
            Payments = paymentRepository;
            ProductAttributes = productAttributeRepository;
            ProductValues = productValueRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
