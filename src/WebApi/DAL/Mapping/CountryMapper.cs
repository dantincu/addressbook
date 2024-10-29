using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public static class CountryMapper
    {
        public static void AddMappings(
            ModelBuilder modelBuilder)
        {
            var entity = EntityMapperCore.AddMappings<Country, int>(
                modelBuilder, false);

            entity.Property(m => m.Id).ValueGeneratedNever(
                ).IsRequired();

            entity.Property(m => m.Name).IsRequired(
                ).HasMaxLength(Constraints.MAX_NAME_LENGTH);

            entity.Property(m => m.Code).IsRequired(
                ).HasMaxLength(Constraints.MAX_CODE_LENGTH);
        }
    }
}
