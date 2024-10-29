using Common.Database;
using Common.Entities;
using DAL.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public abstract class RepositoryBase<TEntity, TPk> : IRepository<TEntity, TPk>
        where TEntity : class
        where TPk : IEquatable<TPk>
    {
        public RepositoryBase(
            AppDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(
                nameof(dbContext));
        }

        public abstract string IdPropName { get; }

        protected AppDbContext DbContext { get; }

        public IAppDbContext GetDbContext() => DbContext;

        public virtual DbSet<TEntity> GetDbSet(
            AppDbContext dbContext) => dbContext.Set<TEntity>();

        public IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter)
        {
            var dbSet = GetDbSet(DbContext);
            var query = dbSet.Where(filter);
            return query;
        }

        public IQueryable<TResult> Query<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector)
        {
            var dbSet = GetDbSet(DbContext);
            var query = dbSet.Where(filter);
            var retQuery = query.Select(selector);
            return retQuery;
        }

        public void Add(TEntity entity)
        {
            var dbSet = GetDbSet(
                DbContext);

            dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            var dbSet = GetDbSet(DbContext);
            var attached = dbSet.Attach(entity);
            attached.State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            var dbSet = GetDbSet(DbContext);
            var attached = dbSet.Attach(entity);
            attached.State = EntityState.Deleted;
        }

        public async Task<int> DeleteAsync(TPk id)
        {
            var dbSet = GetDbSet(DbContext);

            var query = Query(
                GetFilterByIdLambdaExpr(id));

            int result = await query.ExecuteDeleteAsync();
            return result;
        }

        public Task<TEntity?> GetAsync(
            TPk id) => GetAsyncCore(id, false);

        public Task<TEntity> GetRequiredAsync(
            TPk id) => GetAsyncCore(id, true)!;

        public Task<TEntity?> GetFirstAsync(
            Expression<Func<TEntity, bool>> filter) => GetFirstAsyncCore(filter, false);

        public Task<TEntity> GetRequiredFirstAsync(
            Expression<Func<TEntity, bool>> filter) => GetFirstAsyncCore(filter, true)!;

        public Task<TEntity?> GetSingleAsync(
            Expression<Func<TEntity, bool>> filter) => GetSingleAsyncCore(filter, false);

        public Task<TEntity> GetRequiredSingleAsync(
            Expression<Func<TEntity, bool>> filter) => GetSingleAsyncCore(filter, true)!;

        public async Task<TEntity[]> GetQueryAsync(
            Expression<Func<TEntity, bool>> filter)
        {
            var query = Query(filter);
            var entitiesArr = await query.ToArrayAsync();

            return entitiesArr;
        }

        public async Task<TResult[]> GetQueryAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector)
        {
            var query = Query(filter, selector);
            var entitiesArr = await query.ToArrayAsync();

            return entitiesArr;
        }

        protected virtual Expression<Func<TEntity, bool>> GetFilterByIdLambdaExpr(
            TPk id)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, IdPropName);
            var constant = Expression.Constant(id);
            var equality = Expression.Equal(property, constant);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);
            return lambda;
        }

        private async Task<TEntity?> GetAsyncCore(
            TPk id,
            bool required)
        {
            var lambda = GetFilterByIdLambdaExpr(id);

            TEntity? entity = await GetFirstAsyncCore(
                lambda, required);

            return entity;
        }

        private async Task<TEntity?> GetFirstAsyncCore(
            Expression<Func<TEntity, bool>> filter,
            bool required)
        {
            var dbSet = GetDbSet(DbContext);

            TEntity? entity;

            if (required)
            {
                entity = await dbSet.FirstAsync(filter);
            }
            else
            {
                entity = await dbSet.FirstOrDefaultAsync(filter);
            }

            return entity;
        }

        private async Task<TEntity?> GetSingleAsyncCore(
            Expression<Func<TEntity, bool>> filter,
            bool required)
        {
            var dbSet = GetDbSet(DbContext);

            TEntity? entity;

            if (required)
            {
                entity = await dbSet.SingleAsync(filter);
            }
            else
            {
                entity = await dbSet.SingleOrDefaultAsync(filter);
            }

            return entity;
        }
    }

    public class Repository<TEntity, TPk> : RepositoryBase<TEntity, TPk>
        where TEntity : EntityBase<TPk>
        where TPk : IEquatable<TPk>
    {
        public Repository(
            AppDbContext dbContext) : base(dbContext)
        {
        }

        public override string IdPropName => nameof(EntityBase<TPk>.Id);

        protected override Expression<Func<TEntity, bool>> GetFilterByIdLambdaExpr(
            TPk id) => e => e.Id.Equals(id);
    }
}
