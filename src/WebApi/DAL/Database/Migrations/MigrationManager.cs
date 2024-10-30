using Common.Database;
using Common.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Migrations
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase<T>(
            this IHost host) where T : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<T>();
                dbContext.Database.Migrate();
            }

            return host;
        }

        public static async Task AddInitialDataIfReq(
            this IHost host,
            IConfiguration config)
        {
            string? initialDataJsonFile = config.GetValue<string?>(
                "InitialDataJsonFile");

            if (!string.IsNullOrWhiteSpace(initialDataJsonFile))
            {
                var initialData = JsonConvert.DeserializeObject<InitialData>(
                    File.ReadAllText(initialDataJsonFile)) ?? throw new InvalidOperationException(
                        $"File {initialDataJsonFile} does not contain valid json data");
                
                using (var scope = host.Services.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IAddressRepository>();

                    if (await repo.QueryHasAnyAsync(a => true))
                    {
                        var countryRepo = scope.ServiceProvider.GetRequiredService<ICountryRepository>();

                        var allCountries = await countryRepo.GetQueryAsync(
                            countryRepo.QueryInclude(
                                c => true, c => c.Counties));

                        for (int idx = 1; idx <= initialData.TotalNumberOfAddresses; idx++)
                        {
                            var entity = CreateAddressEntity(allCountries, initialData, idx);
                            repo.Add(entity);
                        }

                        await repo.GetDbContext().SaveChangesAsync();
                    }
                }
            }
        }

        private static Address CreateAddressEntity(
            Country[] allCountries,
            InitialData initialData,
            int addressIdx)
        {
            var generator = new RandomValueGenerator(
                addressIdx);

            var address = new Address
            {
                CountryName = generator.GetRandomValue(
                    initialData.CountryNames),
                CityName = generator.GetRandomValue(
                    initialData.CityNames),
                StreetType = generator.GetRandomValue(
                    initialData.StreetTypes),
                StreetName = generator.GetRandomValue(
                    initialData.StreetNames),
                StreetNumber = string.Concat(
                    generator.GetRandomInt(1, 300),
                    generator.GetRandomChar('A', 'F')),
                BlockNumber = string.Concat(
                    generator.GetRandomInt(1, 20),
                    generator.GetRandomChar('A', 'M')),
                StairNumber = string.Concat(
                    generator.GetRandomInt(1, 10),
                    generator.GetRandomChar('A', 'Z')),
                FloorNumber = generator.GetRandomInt(
                    1, 50).ToString(),
                ApartmentNumber = generator.GetRandomInt(
                    1, 1000).ToString(),
                PostalCode = new string(Enumerable.Range(0, 6).Select(
                    idx => generator.GetRandomChar('0', '9')).ToString()),
                CreatedAtUtc = DateTime.UtcNow,
            };

            if (address.CountryName == null)
            {
                address.Country = generator.GetRandomValue(
                    allCountries);

                if (address.Country.Counties.Any())
                {
                    address.County = generator.GetRandomValue(
                        address.Country.Counties);
                }
            }

            if (address.County == null)
            {
                address.CountyName = generator.GetRandomValue(
                    initialData.CountyNames);
            }

            return address;
        }
    }
}
