using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IAddressService : IService
    {
        Task<Address[]> GetFilteredAddressesAsync(
            AddressFilter filter);
    }
}
