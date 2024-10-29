﻿using Common.Services;
using DAL.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.UnitTests
{
    public partial class AppDbContextUnitTest
    {
        [Fact]
        public async Task AddressValidationTest()
        {
            await AssertThrowsBadRequest(async svc =>
                {
                    await svc.DeleteAsync(new ());
                }, true);

            await AssertThrowsBadRequest(async svc =>
            {
                await svc.DeleteAsync(new()
                {
                    PersonId = 1
                });
            });

            await AssertThrowsNotFound(async svc =>
            {
                await svc.DeleteAsync(new()
                {
                    Id = 1
                });
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
            if (resetData)
            {
                using (var svc = GetAddressService())
                {
                    ResetDbContext(
                        (AppDbContext)svc.DbContext);

                    await svc.DbContext.SaveChangesAsync();
                }
            }

            var exc = await Assert.ThrowsAsync<DataAccessException>(
                async () =>
                {
                    using (var svc = GetAddressService())
                    {
                        await asyncAction(svc);
                    }
                });

            Assert.Equal(
                httpStatusCode,
                exc.StatusCode);
        }

        private ExtendedAddressService GetAddressService(
            ) => SvcProv.CreateScope().ServiceProvider.GetRequiredService<ExtendedAddressService>();
    }
}
