using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RicEntityFramework.BaseRepository.Interfaces
{
    public interface IEntityBaseRepository<T> where T : class, new()
    {
        IAsyncEnumerable<T> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        IAsyncEnumerable<T> GetAllAsync();
        int Count();
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
         void Commit();
    }
}
