using Common.Database;
using DAL.Database;
using Dependencies.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Database;
using UnitTests.Services;
using UnitTests.UnitTests;

namespace UnitTests.Dependencies
{
    public class DependencyRoot
    {
        private DependencyRoot()
        {
            var services = new ServiceCollection();

            DependencyRegistration.RegisterAllDependencies(
                services);

            services.AddScoped(svcProv => new MyDbContextFactory(
                ).CreateDbContext([nameof(DbConnectionType.UnitTestDev)]));

            services.AddScoped(svcProv => new ExtendedAddressService(
                svcProv.GetRequiredService<IRepositoryFactory>(),
                svcProv.GetRequiredService<IPersonRepository>(),
                svcProv.GetRequiredService<IAddressRepository>()));

            SvcProv = services.BuildServiceProvider();

            using (var ctx = SvcProv.GetRequiredService<AppDbContext>())
            {
                ctx.Database.Migrate();
            }
        }

        public static Lazy<DependencyRoot> Instance { get; } = new(() => new());

        public IServiceProvider SvcProv { get; }
    }
}
