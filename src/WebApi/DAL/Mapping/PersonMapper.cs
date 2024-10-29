using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public static class PersonMapper
    {
        public static void AddMappings(
            ModelBuilder modelBuilder)
        {
            var entity = EntityMapperCore.AddMappings<Person, int>(
                modelBuilder, true);

            entity.Property(m => m.FirstName).IsRequired(
                ).HasMaxLength(Constraints.MAX_NAME_LENGTH);

            entity.Property(m => m.MiddleName).HasMaxLength(
                Constraints.MAX_NAME_LENGTH);

            entity.Property(m => m.LastName).HasMaxLength(
                Constraints.MAX_NAME_LENGTH).IsRequired();
        }
    }
}
