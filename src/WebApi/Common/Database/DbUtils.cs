using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database
{
    public static class DbUtils
    {
        public static IQueryable<TEntity> IncludeProp<TEntity, TPk, TProperty>(
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath,
            IRepository<TEntity, TPk> repo)
            where TEntity : class
            where TPk : IEquatable<TPk> => repo.QueryInclude(
                query, navigationPropertyPath);

        public static TResult With<TComponent, TResult, TEntity>(
            this IQueryable<TEntity> query,
            TComponent component,
            Func<IQueryable<TEntity>, TComponent, TResult> factory) => factory(query, component);
    }
}
