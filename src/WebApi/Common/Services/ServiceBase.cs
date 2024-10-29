using Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class ServiceBase : IService
    {
        public ServiceBase(
            IAppDbContext appDbContext)
        {
            AppDbContext = appDbContext ?? throw new ArgumentNullException(nameof
                (appDbContext));
        }

        protected IAppDbContext AppDbContext { get; }

        public virtual void Dispose()
        {
            AppDbContext.Dispose();
        }
    }
}
