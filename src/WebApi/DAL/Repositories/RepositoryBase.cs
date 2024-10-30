using Common.Database;
using Common.Entities;
using DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        private static readonly PropertyInfo entryEntityPropInfo;

        static RepositoryBase()
        {
            entryEntityPropInfo = typeof(
                EntityEntry<TEntity>).GetProperties().Single(prop => prop.Name == nameof(
                    EntityEntry<TEntity>.Entity) && prop.DeclaringType == typeof(EntityEntry<TEntity>));
        }

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

        public IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer)
        {
            var dbSet = GetDbSet(DbContext);
            var query = dbSet.Where(filter);
            query = queryTransformer(query);
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

        public IQueryable<TResult> Query<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer,
            Expression<Func<TEntity, TResult>> selector)
        {
            var dbSet = GetDbSet(DbContext);
            var query = dbSet.Where(filter);
            query = queryTransformer(query);
            var retQuery = query.Select(selector);
            return retQuery;
        }

        public IQueryable<TEntity> QueryInclude<TProperty>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        {
            var includeQuery = query.Include(navigationPropertyPath);
            return includeQuery;
        }

        public IQueryable<TEntity> QueryInclude<TProperty>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        {
            var dbSet = GetDbSet(DbContext);
            var query = dbSet.Where(filter);
            var includeQuery = QueryInclude(query, navigationPropertyPath);
            return includeQuery;
        }

        public void Add(TEntity entity)
        {
            var dbSet = GetDbSet(DbContext);
            dbSet.Add(entity);
        }

        public void Update(
            TEntity entity) => AttachCore(
                entity,
                EntityState.Modified);

        public void Delete(
            TEntity entity) => AttachCore(
                entity,
                EntityState.Deleted);

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

        public async Task<TEntity?> GetAsync(
            TPk id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer) => (await GetQueryAsync(
                queryTransformer(Query(GetFilterByIdLambdaExpr(id))).Take(1))).SingleOrDefault();

        public Task<TEntity> GetRequiredAsync(
            TPk id) => GetAsyncCore(id, true)!;

        public async Task<TEntity> GetRequiredAsync(
            TPk id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer) => (await GetQueryAsync(
                queryTransformer(Query(GetFilterByIdLambdaExpr(id))).Take(1))).Single();

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

        public async Task<TEntity[]> GetQueryAsync(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer)
        {
            var query = Query(filter, queryTransformer);
            var entitiesArr = await query.ToArrayAsync();

            return entitiesArr;
        }

        public async Task<TResult[]> GetQueryAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> queryTransformer,
            Expression<Func<TEntity, TResult>> selector)
        {
            var query = Query(filter, queryTransformer, selector);
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

        public async Task<TResult[]> GetQueryAsync<TResult>(
            IQueryable<TResult> query)
        {
            var entitiesArr = await query.ToArrayAsync();
            return entitiesArr;
        }

        public async Task<int> CountQueryAsync(
            Expression<Func<TEntity, bool>> filter)
        {
            var query = Query(filter);
            var count = await query.CountAsync();

            return count;
        }

        public async Task<int> CountQueryAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector)
        {
            var query = Query(filter, selector);
            var count = await query.CountAsync();

            return count;
        }

        public async Task<int> CountQueryAsync<TResult>(
            IQueryable<TResult> query)
        {
            var count = await query.CountAsync();
            return count;
        }

        public async Task<bool> QueryHasAnyAsync(
            Expression<Func<TEntity, bool>> filter)
        {
            var query = Query(filter);
            var hasAny = await query.AnyAsync();

            return hasAny;
        }

        public async Task<bool> QueryHasAnyAsync<TResult>(
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TResult>> selector)
        {
            var query = Query(filter, selector);
            var hasAny = await query.AnyAsync();

            return hasAny;
        }

        public async Task<bool> QueryHasAnyAsync<TResult>(
            IQueryable<TResult> query)
        {
            var hasAny = await query.AnyAsync();
            return hasAny;
        }

        protected virtual Expression<Func<TEntity, TPk>> GetIdRetrieverLambdaExpr()
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, IdPropName);

            var lambda = Expression.Lambda<Func<TEntity, TPk>>(property, parameter);
            return lambda;
        }

        protected virtual Expression<Func<EntityEntry<TEntity>, bool>> GetFilterEntryByIdLambdaExpr(
            TPk id)
        {
            var entityEntryParam = Expression.Parameter(typeof(EntityEntry<TEntity>), "entry");
            var entityProperty = Expression.Property(entityEntryParam, entryEntityPropInfo);
            var idProperty = Expression.Property(entityProperty, IdPropName);
            var idConstant = Expression.Constant(id);
            var equality = Expression.Equal(idProperty, idConstant);

            return Expression.Lambda<Func<EntityEntry<TEntity>, bool>>(equality, entityEntryParam);
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

        private EntityEntry<TEntity> AttachCore(
            TEntity entity,
            EntityState entityState)
        {
            var attached = AttachCore(entity);

            attached.State = entityState;
            return attached;
        }

        private EntityEntry<TEntity> AttachCore(
            TEntity entity)
        {
            var idRetriever = GetIdRetrieverLambdaExpr().Compile();
            var id = idRetriever(entity);
            var idEqLambdaFunc = GetFilterEntryByIdLambdaExpr(id).Compile();
            var allEntries = DbContext.ChangeTracker.Entries<TEntity>();

            var entry = allEntries.FirstOrDefault(
                idEqLambdaFunc);

            if (entry == null)
            {
                var dbSet = GetDbSet(DbContext);
                entry = dbSet.Attach(entity);
            }

            return entry;
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

        protected override Expression<Func<TEntity, TPk>> GetIdRetrieverLambdaExpr(
            ) => e => e.Id;

        protected override Expression<Func<EntityEntry<TEntity>, bool>> GetFilterEntryByIdLambdaExpr(
            TPk id) => e => e.Entity.Id.Equals(id);

        protected override Expression<Func<TEntity, bool>> GetFilterByIdLambdaExpr(
            TPk id) => e => e.Id.Equals(id);
    }
}
