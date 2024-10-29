using Common.Database;
using Common.Entities;
using DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public RepositoryFactory(
            AppDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(
                nameof(dbContext));
        }

        public AppDbContext DbContext { get; }

        public IAppDbContext GetDbContext() => DbContext;

        public IRepository<TEntity, TPk> Repository<TEntity, TPk>()
            where TEntity : EntityBase<TPk>
            where TPk : IEquatable<TPk> => new Repository<TEntity, TPk>(DbContext);
    }
}
