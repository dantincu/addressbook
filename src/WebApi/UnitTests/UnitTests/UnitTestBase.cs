using Common.Database;
using Dependencies.Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Database;
using UnitTests.Dependencies;

namespace UnitTests.UnitTests
{
    public class UnitTestBase
    {
        protected static IServiceProvider SvcProv => DependencyRoot.Instance.Value.SvcProv;
    }
}
