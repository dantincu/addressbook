using Common.Services;
using DAL.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Services;

namespace UnitTests.UnitTests
{
    public partial class AppDbContextUnitTest
    {
        [Fact]
        public async Task AddressValidationTest()
        {
            await AssertThrowsBadRequest(async svc =>
                {
                    await svc.DeleteAsync(0);
                }, true);

            await AssertThrowsNotFound(async svc =>
            {
                await svc.DeleteAsync(1);
            });
        }

        private Task AssertThrowsBadRequest(
            Func<ExtendedAddressService, Task> asyncAction,
            bool resetData = false) => AssertThrowsDataAccessException(
                HttpStatusCode.BadRequest,
                asyncAction,
                resetData);

        private Task AssertThrowsNotFound(
            Func<ExtendedAddressService, Task> asyncAction,
            bool resetData = false) => AssertThrowsDataAccessException(
                HttpStatusCode.NotFound,
                asyncAction,
                resetData);

        private async Task AssertThrowsDataAccessException(
            HttpStatusCode httpStatusCode,
            Func<ExtendedAddressService, Task> asyncAction,
            bool resetData = false)
        {
            var exc = await Assert.ThrowsAsync<DataAccessException>(
                () => PerformTestAsync(asyncAction, resetData));

            Assert.Equal(
                httpStatusCode,
                exc.StatusCode);
        }

        private async Task PerformTestAsync(
            Func<ExtendedAddressService, Task> asyncAction,
            bool resetData = false)
        {
            if (resetData)
            {
                using (var svc = GetAddressService())
                {
                    ResetDbContext(
                        (AppDbContext)svc.DbContext);

                    await svc.DbContext.SaveChangesAsync();
                }
            }

            using (var svc = GetAddressService())
            {
                await asyncAction(svc);
            }
        }

        private ExtendedAddressService GetAddressService(
            ) => SvcProv.CreateScope().ServiceProvider.GetRequiredService<ExtendedAddressService>();
    }
}
