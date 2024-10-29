using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public static class CountyMapper
    {
        public static void AddMappings(
            ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<County>();
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id).ValueGeneratedNever(
                ).IsRequired();

            entity.Property(m => m.Name).IsRequired(
                ).HasMaxLength(Constraints.MAX_NAME_LENGTH);

            entity.Property(m => m.CountryId).IsRequired();

            entity.HasOne(m => m.Country).WithMany(
                m => m.Counties).HasForeignKey(
                m => m.CountryId);
        }
    }
}
