using Common.Entities;
using DAL.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PersonRepository : Repository<Person, int>
    {
        public PersonRepository(
            AppDbContext dbContext) : base(
                dbContext)
        {
        }

        public override DbSet<Person> GetDbSet(
            AppDbContext dbContext) => DbContext.Persons;
    }
}
