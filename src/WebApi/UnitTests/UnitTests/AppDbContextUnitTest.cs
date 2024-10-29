using Common.Database;
using DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnitTests.Database;

namespace UnitTests.UnitTests
{
    public class AppDbContextUnitTest : UnitTestBase
    {
        [Fact]
        public void CountCountriesTest()
        {
            using (var ctx = new MyDbContextFactory(
                ).CreateDbContext([nameof(DbConnectionType.UnitTestDev)]))
            {
                var countriesCount = ctx.Countries.Count();
                Assert.Equal(197, countriesCount);

                var usCountry = ctx.Countries.Include(
                    country => country.Counties).Single(
                    country => country.Code == "US");

                var usStatesCount = usCountry.Counties.Count();
                Assert.Equal(50, usStatesCount);
            }
        }
    }
}