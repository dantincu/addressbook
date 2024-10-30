using Common.Entities;
using Common.Services;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.UnitTests
{
    public partial class AppDbContextUnitTest
    {
        [Fact]
        public async Task AddressServiceTest()
        {
            IAddressService addressService = null!;

            await AddressTestCore(ctx =>
            {
                addressService = GetAddressService();
            }, async (ctx, address1, address2) =>
            {
                var actualAddress1 = await addressService.GetAsync(
                    address1.Id);

                var actualAddress2 = await addressService.GetAsync(
                    address2.Id);

                AssertEntitiesAreEqual(address1, actualAddress1);
                AssertEntitiesAreEqual(address2, actualAddress2);
            }, async (ctx, address1, address2) =>
            {
                await addressService.CreateAsync(address1);
                await addressService.CreateAsync(address2);

                var result = await ctx.SaveChangesAsync();
            }, async (ctx, address1, address2) =>
            {
                await addressService.UpdateAsync(address1);
                await addressService.UpdateAsync(address2);

                await ctx.SaveChangesAsync();
            }, async (ctx, address1, address2) =>
            {
                var person1 = address1.Person;
                var person2 = address2.Person;

                await addressService.DeleteAsync(address1);
                await addressService.DeleteAsync(address2);

                address1.Person ??= person1;
                address2.Person ??= person2;

                await ctx.SaveChangesAsync();
            }, async (ctx, address1, address2) =>
            {
                await AssertThrowsNotFound(async svc => await svc.GetAsync(address1.Id));
                await AssertThrowsNotFound(async svc => await svc.GetAsync(address2.Id));

                var personRepo = new Repository<Person, int>(ctx);

                await AssertHaveBeenDeleted(personRepo, address1.Person);
                await AssertHaveBeenDeleted(personRepo, address2.Person);
            });
        }
    }
}
