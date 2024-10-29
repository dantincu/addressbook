using Common.Database;
using Common.Entities;
using DAL.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> opts) : base(opts)
        {
        }

        public AppDbContext()
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<County> Counties { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            PersonMapper.AddMappings(modelBuilder);
            AddressMapper.AddMappings(modelBuilder);
            CountyMapper.AddMappings(modelBuilder);
            CountryMapper.AddMappings(modelBuilder);
        }

        public Task<int> SaveChangesAsync(
            ) => base.SaveChangesAsync();

        public int ExecuteDelete<TEntity>(
            IQueryable<TEntity> query) => query.ExecuteDelete();
    }
}
