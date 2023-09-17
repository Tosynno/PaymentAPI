using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Helpers
{
    public static class Pagination
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> data, int pageIndex, int pageSize)
            => data.Skip((pageIndex - 1) * pageSize).Take(pageSize);

        public static IQueryable<T> FirstPage<T>(this IQueryable<T> data, int pageSize)
    => data.Take(pageSize);

        public static IQueryable<T> LastPage<T>(this IQueryable<T> data, int pageSize)
  => data.Skip(((data.Count() / pageSize) - 1) * pageSize).Take(pageSize);

        public static int CountOfPages<T>(this IQueryable<T> data, int pageSize)
        {
            var total = data.Count();
            return (total / pageSize) + ((total % pageSize) > 0 ? 1 : 0);
        }

    }
}
