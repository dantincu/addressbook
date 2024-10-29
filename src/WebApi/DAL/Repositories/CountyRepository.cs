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
    public class CountyRepository : Repository<County, int>, ICountyRepository
    {
        public CountyRepository(
            AppDbContext dbContext) : base(
                dbContext)
        {
        }
    }
}
