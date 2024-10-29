using Common.Database;
using DAL.Database;
using Dependencies.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UnitTests.Database
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private readonly IConfiguration config;

        public MyDbContextFactory()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            config = configBuilder.Build();
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            DependencyRegistration.UseSqlite(
                optionsBuilder,
                config,
                nameof(UnitTests),
                GetDbConnectionType(args));

            return new AppDbContext(optionsBuilder.Options);
        }

        private DbConnectionType? GetDbConnectionType(string[] args)
        {
            DbConnectionType? dbConnType = null;
            var firstArg = args.FirstOrDefault();

            if (firstArg != null)
            {
                if (Enum.TryParse<DbConnectionType>(
                    firstArg, out var dbConnTypeValue))
                {
                    dbConnType = dbConnTypeValue;
                }
            }

            return dbConnType;
        }
    }
}
