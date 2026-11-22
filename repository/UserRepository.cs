using Microsoft.EntityFrameworkCore;
using webShop2.Infrastructure.Data;
using webShop2.model;
using webShop2.repository.core;

namespace webShop2.repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            string normalizedEmail =
                email.Trim().ToUpperInvariant();

            return await _dbSet.FirstOrDefaultAsync(
                x => x.NormalizedEmail == normalizedEmail);
        }
    }
}