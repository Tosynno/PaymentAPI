using Microsoft.EntityFrameworkCore;
using PaymentAPI.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Services
{
    public class BaseRepository<TContext, T> : IRepository<TContext, T> where T : class where TContext : DbContext
    {
        protected readonly TContext DbContext;

        public BaseRepository(TContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var res = await DbContext.Set<T>().FirstOrDefaultAsync(predicate);
            return res;
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {

            IQueryable<T> query = DbContext.Set<T>();

            if (disableTracking) query = query.AsNoTracking();



            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);



            if (predicate != null) query = query.Where(predicate);



            if (orderBy != null)

                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();

        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {

            IQueryable<T> query = DbContext.Set<T>();

            if (disableTracking) query = query.AsNoTracking();



            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));



            if (predicate != null) query = query.Where(predicate);



            if (orderBy != null)

                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();

        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {

            return await DbContext.Set<T>().FindAsync(id);

        }

        public virtual async Task<T> GetByIdAsync(int id)
        {

            return await DbContext.Set<T>().FindAsync(id);

        }

        public virtual async Task<T> GetByIdAsync(long id)
        {

            return await DbContext.Set<T>().FindAsync(id);

        }

        public async Task<T> AddAsync(T entity)
        {

            await DbContext.Set<T>().AddAsync(entity);

            await DbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();
        }


        public async Task DeleteAsync(T entity)
        {

            DbContext.Set<T>().Remove(entity);

            await DbContext.SaveChangesAsync();

        }

    }
}
