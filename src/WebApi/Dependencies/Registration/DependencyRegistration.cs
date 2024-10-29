using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Database;
using DAL.Database;
using DAL.Helpers;

namespace Dependencies.Registration
{
    public static class DependencyRegistration
    {
        public const string CONNECTION_STRING_SUFFIX = "Connection";

        public static void RegisterAllDependencies(
            IServiceCollection services,
            IConfiguration config)
        {

        }

        public static void RegisterAllDependencies(
            IServiceCollection services,
            IConfiguration config,
            string? migrationsAssemblyName,
            DbConnectionType? dbConnectionType = null)
        {
            RegisterAppDbContext(
                services,
                config,
                migrationsAssemblyName,
                dbConnectionType);

            RegisterAllDependencies(services, config);
        }

        public static void RegisterAppDbContext(
            IServiceCollection services,
            IConfiguration config,
            string? migrationsAssemblyName,
            DbConnectionType? dbConnectionType = null) =>
            services.AddDbContext<AppDbContext>(
                options => UseSqlite(
                    options,
                    config,
                    migrationsAssemblyName));

        public static DbContextOptionsBuilder UseSqlite(
            DbContextOptionsBuilder options,
            IConfiguration config,
            string? migrationsAssemblyName,
            DbConnectionType? dbConnectionType = null) => UseSqlite(
                options,
                config,
                config.GetConnectionString(
                    GetConnectionStringName(
                        config, dbConnectionType))!,
                migrationsAssemblyName);

        public static DbContextOptionsBuilder UseSqlite(
            DbContextOptionsBuilder options,
            IConfiguration config,
            string connectionString,
            string? migrationsAssemblyName) => options.UseSqlite(
                connectionString,
                options => options.MigrationsAssembly(
                    migrationsAssemblyName));

        public static string GetConnectionStringName(
            IConfiguration config,
            DbConnectionType? dbConnectionType = null)
        {
            dbConnectionType ??= ConnectionStrings.GetDbConnectionType(config);

            var retStr = string.Concat(
                dbConnectionType.ToString(),
                CONNECTION_STRING_SUFFIX);

            return retStr;
        }
    }
}
