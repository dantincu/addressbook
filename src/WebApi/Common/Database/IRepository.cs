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

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter);

        IQueryable<TResult> Query<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector);

        Task<TEntity?> GetAsync(TPk id);
        Task<TEntity> GetRequiredAsync(TPk id);

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

        Task<TResult[]> GetQueryAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector);
    }
}
