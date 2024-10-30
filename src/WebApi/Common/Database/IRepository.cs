using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database
{
    public interface IRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IEquatable<TPk>
    {
        string IdPropName { get; }

        IAppDbContext GetDbContext();

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<int> DeleteAsync(TPk id);

        IQueryable<TEntity> QueryInclude<TProperty>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath);

        IQueryable<TEntity> QueryInclude<TProperty>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath);

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter);

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer);

        IQueryable<TResult> Query<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector);

        IQueryable<TResult> Query<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer,
            Expression<Func<TEntity, TResult>> selector);

        Task<TEntity?> GetAsync(TPk id);

        Task<TEntity?> GetAsync(TPk id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer);

        Task<TEntity> GetRequiredAsync(TPk id);

        Task<TEntity> GetRequiredAsync(TPk id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer);

        Task<TEntity?> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<TEntity> GetRequiredFirstAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<TEntity?> GetSingleAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<TEntity> GetRequiredSingleAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<TEntity[]> GetQueryAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<TEntity[]> GetQueryAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer);

        Task<TResult[]> GetQueryAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector);

        Task<TResult[]> GetQueryAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer,
            Expression<Func<TEntity, TResult>> selector);

        Task<TResult[]> GetQueryAsync<TResult>(
            IQueryable<TResult> query);

        Task<int> CountQueryAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<int> CountQueryAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector);

        Task<int> CountQueryAsync<TResult>(
            IQueryable<TResult> query);

        Task<bool> QueryHasAnyAsync(
            Expression<Func<TEntity, bool>> filter);

        Task<bool> QueryHasAnyAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector);

        Task<bool> QueryHasAnyAsync<TResult>(
            IQueryable<TResult> query);
    }
}
