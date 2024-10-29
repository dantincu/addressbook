using Common.Database;
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

        public async Task<Address[]> GetFilteredAddressesAsync(
            AddressFilter filter)
        {
            var addressesArr = await AddressRepository.GetQueryAsync(
                addr => true);

            return addressesArr;
        }

        public async Task<Address> GetAsync(int id)
        {
            var entity = await AddressRepository.GetAsync(id);

            ThrowNotFoundIfTrue(
                entity == null);

            return entity!;
        }

        public async Task<Address> CreateAsync(
            Address entity)
        {
            ValidateAddress(entity, true);
            AddressRepository.Add(entity);
            await AppDbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Address> UpdateAsync(
            Address entity)
        {
            ValidateAddress(entity, false);
            AddressRepository.Update(entity);
            int result = await AppDbContext.SaveChangesAsync();

            if (result <= 0)
            {
                throw new DataAccessException(
                    HttpStatusCode.NotFound);
            }

            return entity;
        }

        public async Task<Address> DeleteAsync(
            Address entity)
        {
            ValidateEntityCore<Address, int>(
                entity, false);

            ThrowBadRequestIfTrue(
                entity.Person != null);

            ThrowBadRequestIfTrue(
                entity.PersonId != 0);

            entity = (await AddressRepository.GetAsync(entity.Id))!;

            ThrowNotFoundIfTrue(
                entity == null);

            AddressRepository.Delete(entity!);

            PersonRepository.Delete(
                new Person
                {
                    Id = entity.PersonId,
                });

            int result = await AppDbContext.SaveChangesAsync();

            ThrowNotFoundIfTrue(
                result == 0);

            return entity;
        }

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

            ValidateAllRequiredStrings(
                [ entity.CountryName ], entity.CountryId == null);

            ValidateAllRequiredStrings(
                [entity.CountyName], entity.CountyId == null);

            ThrowBadRequestIfTrue(
                entity.Person == null);

            ValidatePerson(
                entity.Person!,
                isNew);
        }

        private void ValidatePerson(
            Person entity,
            bool isNew)
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

        private void ThrowBadRequestIfTrue(
            bool condition) => ThrowIfTrue(
                condition,
                HttpStatusCode.BadRequest);

        private void ThrowNotFoundIfTrue(
            bool condition) => ThrowIfTrue(
                condition,
                HttpStatusCode.NotFound);

        private void ThrowIfTrue(
            bool condition,
            HttpStatusCode httpStatusCode)
        {
            if (condition)
            {
                throw new DataAccessException(
                    httpStatusCode);
            }
        }
    }
}
