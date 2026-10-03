using System;
using System.Collections.Generic;
using System.Text;
using webShop2.model;

namespace webShop2.repository.core
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(int id);

        Task<List<T>> GetAllAsync();

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
