using BL.Services;
using Common.Database;
using DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Services
{
    public class ExtendedAddressService : AddressService
    {
        public ExtendedAddressService(
            IRepositoryFactory repositoryFactory,
            IPersonRepository personRepository,
            IAddressRepository addressRepository) : base(
                repositoryFactory,
                personRepository,
                addressRepository)
        {
        }

        public IAppDbContext DbContext => AppDbContext;
    }
}
