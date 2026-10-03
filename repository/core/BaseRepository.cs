using Microsoft.EntityFrameworkCore;
using webShop2.Infrastructure.Data;
using webShop2.model;

namespace webShop2.repository.core;

public class BaseRepository<T> : IBaseRepository<T>
    where T : Entity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet
            .ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}