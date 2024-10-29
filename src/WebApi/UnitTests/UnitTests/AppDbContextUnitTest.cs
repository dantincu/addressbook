using Common.Database;
using Common.Entities;
using DAL.Database;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using UnitTests.Database;
using static UnitTests.UnitTests.AppDbContextUnitTest;

namespace UnitTests.UnitTests
{
    public class AppDbContextUnitTest : UnitTestBase
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
                FirstName = "George",
                LastName = "Smith",
            };

            BasicRepository<Person, int> basicRepo = null!;
            Repository<Person, int> repo = null!;

            Action<AppDbContext> assignRepos = (ctx) =>
            {
                /* basicRepo = new BasicPersonRepository(ctx);
                repo = new PersonRepository(ctx); */

                basicRepo = new BasicRepository<Person, int>(ctx);
                repo = new Repository<Person, int>(ctx);
            };

            Func<BasicRepository<Person, int>, Repository<Person, int>, Task> assertAction = async (
                basicRepo, repo) =>
            {
                var allPersons = await repo.GetFilteredAsync(e => true);

                await GetAndAssertAreEqual(
                    repo, person1, AssertEntitiesAreEqual);

                await GetAndAssertAreEqual(
                    basicRepo, person2, AssertEntitiesAreEqual);
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

                person2.FirstName = "Gary";
                person2.MiddleName = "Archibald";
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
            Person entity1,
            Person entity2)
        {
            Assert.Equal(entity1.Id, entity2.Id);
            Assert.Equal(entity1.FirstName, entity2.FirstName);
            Assert.Equal(entity1.MiddleName, entity2.MiddleName);
            Assert.Equal(entity1.LastName, entity2.LastName);
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

        public class BasicPersonRepository : BasicRepository<Person, int>
        {
            public BasicPersonRepository(
                AppDbContext dbContext) : base(
                    dbContext)
            {
            }

            public override DbSet<Person> GetDbSet(
                AppDbContext dbContext) => DbContext.Persons;
        }
    }
}