using webShop2.model;

namespace webShop2.repository.core
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);

        Task<List<User>> GetAllAsync();

        Task<User?> GetByEmailAsync(string email);
    }
}