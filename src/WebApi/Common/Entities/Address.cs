using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Address : EntityBase<int>
    {
        public string? CountryName { get; set; }
        public string? CountyName { get; set; }
        public string CityName { get; set; }

        public string? StreetType { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string? PostalCode { get; set; }

        public string? BlockNumber { get; set; }
        public string? StairNumber { get; set; }
        public string? FloorNumber { get; set; }
        public string? ApartmentNumber { get; set; }

        public int PersonId { get; set; }
        public int? CountryId { get; set; }
        public int? CountyId { get; set; }

        public Country? Country { get; set; }
        public County? County { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }

        public Person Person { get; set; }
    }
}
