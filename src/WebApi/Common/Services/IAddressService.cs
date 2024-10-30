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

        Task<Address> GetAsync(
            int id);

        Task<Address> CreateAsync(
            Address entity);

        Task<Address> UpdateAsync(
            Address entity);

        Task<Address> DeleteAsync(
            Address entity);
    }
}
