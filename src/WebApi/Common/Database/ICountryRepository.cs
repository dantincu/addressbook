﻿using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database
{
    public interface ICountryRepository : IRepository<Country, int>
    {
    }
}
