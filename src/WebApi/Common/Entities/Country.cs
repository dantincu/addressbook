using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Country : EntityBase<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public List<County> Counties { get; set; }

        public List<Address> Addresses { get; set; }
    }
}
