using webShop2.dto.user;
using webShop2.model;
using webShop2.repository.core;
using webShop2.services.core;

namespace webShop2.services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _unitOfWork.Users.GetByEmailAsync(email);
        }

        public async Task<User> CreateAsync(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        public async Task<User?> UpdateAsync(int id, User userUpdate)
        {
            User? user =
                await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            _unitOfWork.Users.Update(userUpdate);
            await _unitOfWork.SaveChangesAsync();

            return userUpdate;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            User? user =
                await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
