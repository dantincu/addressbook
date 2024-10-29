using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database
{
    public interface IAppDbContext : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();

        int ExecuteDelete<TEntity>(
            IQueryable<TEntity> query);
    }
}
