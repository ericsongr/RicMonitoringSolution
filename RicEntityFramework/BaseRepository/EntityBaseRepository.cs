using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RicEntityFramework.BaseRepository.Interfaces;

namespace RicEntityFramework.BaseRepository
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T>
        where T : class, new()
    {

        public RicDbContext Context;

        #region Properties
        public EntityBaseRepository(RicDbContext context)
        {
            Context = context;
        }
        #endregion

        public virtual IAsyncEnumerable<T> GetAllAsync()
        {
            return Context.Set<T>().AsAsyncEnumerable();
        }

        public virtual int Count()
        {
            return Context.Set<T>().Count();
        }
        public virtual IAsyncEnumerable<T> AllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query.AsAsyncEnumerable();
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T> GetSingleIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Context.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).ToList();

        }
        public virtual void Add(T entity)
        {
            EntityEntry dbEntityEntry = Context.Entry<T>(entity);
            Context.Set<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = Context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;

        }
        public virtual void Delete(T entity)
        {
            EntityEntry dbEntityEntry = Context.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Deleted;
        }

        public virtual void DeleteWhere(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> entities = Context.Set<T>().Where(predicate);

            foreach (var entity in entities)
            {
                Context.Entry<T>(entity).State = EntityState.Deleted;
            }
        }

        public virtual void Commit()
        {
           Context.SaveChanges();
        }
    }
}
