using Common.Database;
using Common.Entities;
using Common.Services;
using DAL.Database;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using UnitTests.Database;

namespace UnitTests.UnitTests
{
    public partial class AppDbContextUnitTest : UnitTestBase
    {
        [Fact]
        public void CountCountriesTest()
        {
            using (var ctx = new MyDbContextFactory(
                ).CreateDbContext([nameof(
                    DbConnectionType.UnitTestDev)]))
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

        [Fact]
        public async Task PersonTest()
        {
            var person1 = new Person
            {
                FirstName = "John",
                LastName = "Doe",
            };

            var person2 = new Person
            {
                FirstName = "Patrick",
                LastName = "Smith",
            };

            BasicRepository<Person, int> basicRepo = null!;
            Repository<Person, int> repo = null!;

            Action<AppDbContext> assignRepos = (ctx) =>
            {
                basicRepo = new(ctx);
                repo = new(ctx);
            };

            Func<BasicRepository<Person, int>, Repository<Person, int>, Task> assertAction = async (
                basicRepo, repo) =>
            {
                var allPersons = await repo.GetQueryAsync(e => true);

                await GetAndAssertAreEqual(
                    repo, person1, AssertEntitiesAreEqualCore);

                await GetAndAssertAreEqual(
                    basicRepo, person2, AssertEntitiesAreEqualCore);
            };

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);
                basicRepo.Add(person1);
                repo.Add(person2);

                var result = await ctx.SaveChangesAsync();

                Assert.NotEqual(0, person1.Id);
                Assert.NotEqual(0, person2.Id);
                Assert.NotEqual(person1.Id, person2.Id);
            }, true);

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);
                await assertAction(basicRepo, repo);
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);

                person1.FirstName = "Jonny";
                person1.MiddleName = "Archibald";
                person1.LastName = "Dawson";

                person2.FirstName = "Patty";
                person2.MiddleName = "Von";
                person2.LastName = "Smithson";

                basicRepo.Update(person1);
                repo.Update(person2);

                await ctx.SaveChangesAsync();
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);
                await assertAction(basicRepo, repo);
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);

                repo.Delete(person1);
                basicRepo.Delete(person2);

                await ctx.SaveChangesAsync();
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);
                await AssertHaveBeenDeleted(repo, person1);
                await AssertHaveBeenDeleted(basicRepo, person2);
            });
        }

        [Fact]
        public async Task AddressTest()
        {
            var address1 = new Address
            {
                CountryName = "Romania",
                CountyName = "Bucuresti",
                CityName = "Sector 4",
                StreetType = "Bd.",
                StreetName = "Gheorghe Ionescu",
                StreetNumber = "123B",
                BlockNumber = "M",
                StairNumber = "F",
                ApartmentNumber = "432",
                PostalCode = "000999",
                CreatedAtUtc = DateTime.UtcNow.AddMinutes(-1),
                LastModifiedAtUtc = DateTime.UtcNow
            };

            var address2 = new Address
            {
                CityName = "Montgomery",
                StreetType = "St.",
                StreetName = "Broadway",
                StreetNumber = "65",
                PostalCode = "999000",
                CreatedAtUtc = DateTime.UtcNow.AddMinutes(-10),
                LastModifiedAtUtc = DateTime.UtcNow.AddMinutes(-5)
            };

            var person1 = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Addresses = [address1]
            };

            var person2 = new Person
            {
                FirstName = "Patrick",
                LastName = "Smith",
                Addresses = [address2]
            };

            BasicRepository<Person, int> basicRepo = null!;
            Repository<Person, int> repo = null!;
            Repository<Address, int> addrRepo = null!;

            Action<AppDbContext> assignRepos = (ctx) =>
            {
                basicRepo = new(ctx);
                repo = new(ctx);
                addrRepo = new(ctx);
            };

            Func<BasicRepository<Person, int>, Repository<Person, int>, Task> assertAction = async (
                basicRepo, repo) =>
            {
                var allPersons = await repo.GetQueryAsync(e => true);

                await GetAndAssertAreEqual(
                    repo, person1, AssertEntitiesAreEqual);

                await GetAndAssertAreEqual(
                    basicRepo, person2, AssertEntitiesAreEqual);
            };

            await PerformTestAsync(async ctx =>
            {
                var usCountry = ctx.Countries.First(
                    c => c.Code == "US");

                var usState = ctx.Counties.First(
                    c => c.Country.Code == "US");

                address2.Country = usCountry;
                address2.County = usState;

                assignRepos(ctx);
                basicRepo.Add(person1);
                repo.Add(person2);

                var result = await ctx.SaveChangesAsync();

                Assert.NotEqual(0, person1.Id);
                Assert.NotEqual(0, person2.Id);
                Assert.NotEqual(0, address1.Id);
                Assert.NotEqual(0, address2.Id);
                Assert.NotEqual(person1.Id, person2.Id);
                Assert.NotEqual(address1.Id, address2.Id);
            }, true);

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);
                await assertAction(basicRepo, repo);
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);

                address1.CountryName += " Country";
                address1.CountyName += " County";
                address1.CityName += " City";

                address2.CountryName = address2.Country.Name + " Country";
                address2.CountyName = address2.County.Name + " County";
                address2.CityName += " City";

                basicRepo.Update(person1);
                repo.Update(person2);

                await ctx.SaveChangesAsync();
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);
                await assertAction(basicRepo, repo);
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);

                repo.Delete(person1);
                basicRepo.Delete(person2);

                await ctx.SaveChangesAsync();
            });

            await PerformTestAsync(async ctx =>
            {
                assignRepos(ctx);
                await AssertHaveBeenDeleted(repo, person1);
                await AssertHaveBeenDeleted(basicRepo, person2);
                await AssertHaveBeenDeleted(addrRepo, address1);
                await AssertHaveBeenDeleted(addrRepo, address2);
            });
        }

        private async Task<TEntity> GetAndAssertAreEqual<TEntity, TPk>(
            RepositoryBase<TEntity, TPk> repo,
            TEntity srcEntity,
            Action<TEntity, TEntity> areEqualAssertAction)
            where TEntity : EntityBase<TPk>
            where TPk : IEquatable<TPk>
        {
            var retEntity = await repo.GetRequiredAsync(srcEntity.Id);
            areEqualAssertAction(srcEntity, retEntity);
            return retEntity;
        }

        private async Task AssertHaveBeenDeleted<TEntity, TPk>(
            RepositoryBase<TEntity, TPk> repo,
            TEntity srcEntity)
            where TEntity : EntityBase<TPk>
            where TPk : IEquatable<TPk>
        {
            var retEntity = await repo.GetAsync(srcEntity.Id);
            Assert.Null(retEntity);
        }

        private async Task PerformTestAsync(
            Func<AppDbContext, Task> testAction,
            bool resetData = false)
        {
            if (resetData)
            {
                using (var ctx = new MyDbContextFactory(
                ).CreateDbContext([nameof(
                    DbConnectionType.UnitTestDev)]))
                {
                    ResetDbContext(ctx);
                    await ctx.SaveChangesAsync();
                }
            }

            using (var ctx = new MyDbContextFactory(
                ).CreateDbContext([nameof(
                    DbConnectionType.UnitTestDev)]))
            {
                await testAction(ctx);
            }
        }

        private void ResetDbContext(
            AppDbContext ctx)
        {
            ctx.Addresses.ExecuteDelete();
            ctx.Persons.ExecuteDelete();
        }

        private void AssertEntitiesAreEqual(
            Address entity1,
            Address entity2)
        {
            AssertEntitiesAreEqualCore(entity1, entity2);

            AssertNestedObjectsAreEqual(
                entity1.Country,
                entity2.Country,
                AssertEntitiesAreEqual,
                false);

            AssertNestedObjectsAreEqual(
                entity1.County,
                entity2.County,
                AssertEntitiesAreEqual);

            AssertNestedObjectsAreEqual(
                entity1.Person,
                entity2.Person,
                AssertEntitiesAreEqual);
        }

        private void AssertEntitiesAreEqual(
            Person entity1,
            Person entity2)
        {
            AssertEntitiesAreEqualCore(entity1, entity2);
        }

        private void AssertEntitiesAreEqual(
            Country entity1,
            Country entity2)
        {
            AssertEntitiesAreEqualCore(entity1, entity2);
        }

        private void AssertEntitiesAreEqual(
            County entity1,
            County entity2)
        {
            AssertEntitiesAreEqualCore(entity1, entity2);

            AssertNestedObjectsAreEqual(
                entity1.Country,
                entity2.Country,
                AssertEntitiesAreEqual);
        }

        private void AssertEntitiesAreEqualCore(
            Address address1,
            Address address2)
        {
            Assert.Equal(address1.Id, address2.Id);
            Assert.Equal(address1.CountryName, address2.CountryName);
            Assert.Equal(address1.CountyName, address2.CountyName);
            Assert.Equal(address1.CityName, address2.CityName);
            Assert.Equal(address1.StreetType, address2.StreetType);
            Assert.Equal(address1.StreetName, address2.StreetName);
            Assert.Equal(address1.StreetNumber, address2.StreetNumber);
            Assert.Equal(address1.PostalCode, address2.PostalCode);
            Assert.Equal(address1.BlockNumber, address2.BlockNumber);
            Assert.Equal(address1.StairNumber, address2.StairNumber);
            Assert.Equal(address1.FloorNumber, address2.FloorNumber);
            Assert.Equal(address1.ApartmentNumber, address2.ApartmentNumber);
            Assert.Equal(address1.PersonId, address2.PersonId);
            Assert.Equal(address1.CountryId, address2.CountryId);
            Assert.Equal(address1.CountyId, address2.CountyId);
            Assert.Equal(address1.CreatedAtUtc, address2.CreatedAtUtc);
            Assert.Equal(address1.LastModifiedAtUtc, address2.LastModifiedAtUtc);
        }

        private void AssertEntitiesAreEqualCore(
            Person entity1,
            Person entity2)
        {
            Assert.Equal(entity1.Id, entity2.Id);
            Assert.Equal(entity1.FirstName, entity2.FirstName);
            Assert.Equal(entity1.MiddleName, entity2.MiddleName);
            Assert.Equal(entity1.LastName, entity2.LastName);
        }

        private void AssertEntitiesAreEqualCore(
            Country entity1,
            Country entity2)
        {
            Assert.Equal(entity1.Id, entity2.Id);
            Assert.Equal(entity1.Name, entity2.Name);
            Assert.Equal(entity1.Code, entity2.Code);
        }

        private void AssertEntitiesAreEqualCore(
            County entity1,
            County entity2)
        {
            Assert.Equal(entity1.Id, entity2.Id);
            Assert.Equal(entity1.Name, entity2.Name);
        }

        private void AssertNestedObjectsAreEqual<TObj>(
            TObj? obj1,
            TObj? obj2,
            Action<TObj, TObj> areEqualCallback,
            bool allowNull = true)
        {
            if (!allowNull)
            {
                Assert.NotNull(obj1);
            }

            if (obj1 is null)
            {
                Assert.Null(obj2);
            }
            else
            {
                Assert.NotNull(obj2);
                areEqualCallback(obj1, obj2);
            }
        }

        public class BasicRepository<TEntity, TPk> : RepositoryBase<TEntity, TPk>
            where TEntity : EntityBase<TPk>
            where TPk : IEquatable<TPk>
        {
            public BasicRepository(
                AppDbContext dbContext) : base(dbContext)
            {
            }

            public override string IdPropName => nameof(EntityBase<TPk>.Id);
        }
    }
}