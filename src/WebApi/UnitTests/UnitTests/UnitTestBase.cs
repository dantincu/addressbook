using Common.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Database;

namespace UnitTests.UnitTests
{
    public class UnitTestBase
    {
        static UnitTestBase()
        {
            using (var ctx = new MyDbContextFactory(
                ).CreateDbContext([nameof(DbConnectionType.UnitTestDev)]))
            {
                ctx.Database.Migrate();
            }
        }

        public UnitTestBase()
        {

        }
    }
}
