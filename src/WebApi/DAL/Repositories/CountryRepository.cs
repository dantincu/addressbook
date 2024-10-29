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
    public class CountryRepository : Repository<Country, int>, ICountryRepository
    {
        public CountryRepository(
            AppDbContext dbContext) : base(
                dbContext)
        {
        }
    }
}
