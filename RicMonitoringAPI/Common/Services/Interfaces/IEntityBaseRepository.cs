using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RicMonitoringAPI.Services.Interfaces
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        IAsyncEnumerable<T> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        IAsyncEnumerable<T> GetAllAsync();
        int Count();
        Task<T> GetSingleAsync(int id);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
         void Commit();
    }
}
