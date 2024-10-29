using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity, TPk> Repository<TEntity, TPk>()
            where TEntity : EntityBase<TPk>
            where TPk : IEquatable<TPk>;
    }
}
