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

    //public async Task<List<T>> GetAllAsync()
    //{
    //    List<T> entities = await _dbSet.ToListAsync();

    //    Console.WriteLine("BaseRepository broj elemenata: " + entities.Count);

    //    return entities;
    //}
    public async Task<List<T>> GetAllAsync()
    {
        Console.WriteLine("========== BASE REPOSITORY ==========");
        Console.WriteLine("Entity type: " + typeof(T).Name);

        System.Data.Common.DbConnection connection =
            _context.Database.GetDbConnection();

        Console.WriteLine("Database: " + connection.Database);
        Console.WriteLine("Data source: " + connection.DataSource);

        Console.WriteLine("SQL koji EF Core izvršava:");
        Console.WriteLine(_dbSet.ToQueryString());

        List<T> entities = await _dbSet.ToListAsync();

        Console.WriteLine("Broj dobijenih redova: " + entities.Count);

        foreach (T entity in entities)
        {
            Console.WriteLine("Dobijen ID: " + entity.Id);
        }

        Console.WriteLine("======================================");

        return entities;
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