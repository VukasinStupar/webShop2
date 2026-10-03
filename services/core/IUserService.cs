using webShop2.dto.user;
using webShop2.model;

namespace webShop2.services.core;

public interface IUserService
{
     Task<User?> GetByIdAsync(int id);
     Task<List<User>> GetAllAsync();
     Task<User?> GetByEmailAsync(string email);

     Task<User> CreateAsync(User user);
     Task<User?> UpdateAsync(int id, User user);
     Task<bool> DeleteAsync(int id);
}