using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database.Migrations
{
    public class InitialData
    {
        public int TotalNumberOfAddresses { get; set; }
        public string[] Names { get; set; }
        public string[] SurNames { get; set; }
        public string[] CountyNames { get; set; }
        public string[] CityNames { get; set; }
        public string[] StreetTypes { get; set; }
        public string[] StreetNames { get; set; }
    }
}
