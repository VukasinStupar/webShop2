using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;
using Microsoft.EntityFrameworkCore;

namespace webShop2.repository
{
    public class UserRepository
     : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
