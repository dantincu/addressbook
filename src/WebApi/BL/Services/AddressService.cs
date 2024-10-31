using Common.Database;
using Common.DTOs;
using Common.Entities;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class AddressService : ServiceBase, IAddressService
    {
        public AddressService(
            IRepositoryFactory repositoryFactory,
            IPersonRepository personRepository,
            IAddressRepository addressRepository) : base(
                repositoryFactory)
        {
            PersonRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            AddressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
        }

        protected IPersonRepository PersonRepository { get; }
        protected IAddressRepository AddressRepository { get; }

        public async Task<AddressSummary[]> GetFilteredAddressesAsync(
            AddressFilter filter)
        {
            var addressesArr = await AddressRepository.GetQueryAsync(
                addr => true,
                query => query.Skip(filter.SkipCount).Take(filter.TakeCount),
                addr => new AddressSummary
                {
                    Id = addr.Id,
                    FirstName = addr.Person.FirstName,
                    MiddleName = addr.Person.MiddleName,
                    LastName = addr.Person.LastName,
                    CountryName = addr.CountryName ?? addr.Country.Name,
                    CountyName = addr.CountyName ?? addr.County.Name,
                    CityName = addr.CityName,
                    CreatedAtUtc = addr.CreatedAtUtc,
                    LastModifiedAtUtc = addr.LastModifiedAtUtc
                });

            foreach (var address in addressesArr)
            {
                address.FullName = string.Join(
                    " ", new string?[]
                    {
                        address.FirstName,
                        address.MiddleName,
                        address.LastName
                    }.Select(name => name?.Trim()).Where(
                        name => !string.IsNullOrEmpty(name)));

                address.FirstName = null;
                address.MiddleName = null;
                address.LastName = null;
            }

            return addressesArr;
        }

        public async Task<Address> GetAsync(int id)
        {
            var entity = await GetAsyncCore(id);

            ThrowNotFoundIfTrue(
                entity == null);

            if (entity.Country != null)
            {
                entity.CountryName = entity.Country.Name;
                entity.Country = null;
            }

            if (entity.County != null)
            {
                entity.CountyName = entity.County.Name;
                entity.County = null;
            }

            entity.Person.Addresses = null;

            return entity!;
        }

        public async Task<Address> CreateAsync(
            Address entity)
        {
            ValidateAddress(entity, true);
            entity.CreatedAtUtc = DateTime.UtcNow;
            AddressRepository.Add(entity);
            await SaveChangesAsync();
            entity = (await GetAsyncCore(entity.Id))!;
            return entity;
        }

        public async Task<Address> UpdateAsync(
            Address entity)
        {
            ValidateAddress(entity, false);
            entity.PersonId = await GetPersonIdAsync(entity.Id);
            ThrowNotFoundIfTrue(entity.PersonId == 0);
            entity.LastModifiedAtUtc = DateTime.UtcNow;
            AddressRepository.Update(entity);
            await SaveChangesAsync();
            entity = (await GetAsyncCore(entity.Id))!;
            return entity;
        }

        public async Task<Address> DeleteAsync(int id)
        {
            var entity = new Address { Id = id };

            ValidateEntityCore<Address, int>(
                entity, false);

            var personId = await GetPersonIdAsync(id);

            ThrowNotFoundIfTrue(
                personId == 0);

            AddressRepository.Delete(entity!);

            PersonRepository.Delete(
                new Person
                {
                    Id = personId,
                });

            await SaveChangesAsync();
            return entity;
        }

        private Task<Address?> GetAsyncCore(
            int addressId) => AddressRepository.GetAsync(
                addressId, query => query.IncludeProp(
                    a => a.Country, AddressRepository).IncludeProp(
                    a => a.County, AddressRepository).IncludeProp(
                    a => a.Person, AddressRepository));

        private async Task<int> GetPersonIdAsync(
            int addressId) => (await AddressRepository.GetQueryAsync(
                AddressRepository.Query(
                    a => a.Id == addressId,
                    a => a.PersonId).Take(1))).SingleOrDefault();

        private void ValidateAddress(
            Address entity,
            bool isNew)
        {
            ValidateEntityCore<Address, int>(entity, isNew);

            ValidateAllRequiredStrings(
                [ entity.CityName,
                entity.StreetType,
                entity.StreetName,
                entity.StreetNumber ]);

            entity.CountryId ??= entity.Country?.Id;
            entity.CountyId ??= entity.County?.Id;

            ValidateAllRequiredStrings(
                [ entity.CountryName ], entity.CountryId == null);

            ValidateAllRequiredStrings(
                [entity.CountyName], entity.CountyId == null);

            ThrowBadRequestIfTrue(
                entity.Person == null);

            ValidatePerson(
                entity.Person!,
                null);

            ClearAddressNestedEntities(entity, isNew);
        }

        private void ValidatePerson(
            Person entity,
            bool? isNew)
        {
            ValidateEntityCore<Person, int>(
                entity, isNew);

            ValidateAllRequiredStrings(
                [ entity.FirstName,
                entity.LastName ]);
        }

        private void ValidateAllRequiredStrings(
            string?[] valuesArr,
            bool condition = true)
        {
            ThrowBadRequestIfTrue(
                condition && valuesArr.Any(
                string.IsNullOrWhiteSpace));
        }

        private void ClearAddressNestedEntities(
            Address entity,
            bool isNew)
        {
            entity.Country = null;
            entity.County = null;

            if (!isNew)
            {
                entity.Person = null;
            }
        }
    }
}
