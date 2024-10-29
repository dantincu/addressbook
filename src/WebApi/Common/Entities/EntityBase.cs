using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class EntityBase<TPk>
        where TPk : IEquatable<TPk>
    {
        public TPk Id { get; set; }
    }
}
