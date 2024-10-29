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

        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter);

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

        Task<TEntity[]> GetFilteredAsync(
            Expression<Func<TEntity, bool>> filter);
    }
}
