using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Interface
{
    public interface IRepository<TContext, T> where T : class where TContext : DbContext
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,

            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,

            string includeString = null,

            bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,

            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,

            List<Expression<Func<T, object>>> includes = null,

            bool disableTracking = true);

        Task<T> GetByIdAsync(Guid id);

        Task<T> GetByIdAsync(long id);
        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
