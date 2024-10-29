using Common.Database;
using Common.Entities;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ServiceBase : IService
    {
        public ServiceBase(
            IRepositoryFactory repositoryFactory)
        {
            RepositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof
                (repositoryFactory));

            AppDbContext = RepositoryFactory.GetDbContext();
        }

        protected IRepositoryFactory RepositoryFactory { get; }
        protected IAppDbContext AppDbContext { get; }

        public virtual void Dispose()
        {
            AppDbContext.Dispose();
        }

        protected void ValidateEntityCore<TEntity, TPk>(
            TEntity entity,
            bool isNew)
            where TEntity : EntityBase<TPk>
            where TPk : IEquatable<TPk>
        {
            if (entity.Id.Equals(default) != isNew)
            {
                throw new DataAccessException(
                    HttpStatusCode.BadRequest);
            }
        }
    }
}
