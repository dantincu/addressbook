using Common.Database;
using Common.Entities;
using Common.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CataloguesService : ServiceBase, ICataloguesService
    {
        public CataloguesService(
            IRepositoryFactory repositoryFactory,
            ICountryRepository countryRepository,
            ICountyRepository countyRepository) : base(
                repositoryFactory)
        {
            CountryRepository = countryRepository ?? throw new ArgumentNullException(
                nameof(countryRepository));

            CountyRepository = countyRepository ?? throw new ArgumentNullException(
                nameof(countyRepository));
        }

        protected ICountryRepository CountryRepository { get; }
        protected ICountyRepository CountyRepository { get; }

        public Task<Country[]> GetAllCountries(
            ) => CountryRepository.GetQueryAsync(c => true);

        public async Task<County[]> GetCounties(
            string countryCode)
        {
            ThrowBadRequestIfTrue(
                string.IsNullOrWhiteSpace(countryCode));

            countryCode = countryCode.ToUpper();

            var country = (await CountryRepository.GetQueryAsync(
                c => c.Code == countryCode,
                (IQueryable<Country> query) => query.IncludeProp(
                    c => c.Counties, CountryRepository).Take(1))).SingleOrDefault();

            ThrowNotFoundIfTrue(
                country == null);

            var result = country!.Counties.ToArray();

            foreach (var county in result)
            {
                county.Country = null;
                county.CountryId = 0;
            }

            return result;
        }
    }
}
