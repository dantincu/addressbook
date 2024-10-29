using Common.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public static class AddressMapper
    {
        public static void AddMappings(
            ModelBuilder modelBuilder)
        {
            var entity = EntityMapperCore.AddMappings<Address, int>(
                modelBuilder, true);

            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).IsRequired();
            entity.Property(m => m.PersonId).IsRequired();

            entity.Property(m => m.CountryName).HasMaxLength(
                Constraints.MAX_NAME_LENGTH);

            entity.Property(m => m.CountyName).HasMaxLength(
                Constraints.MAX_NAME_LENGTH);

            entity.Property(m => m.CityName).HasMaxLength(
                Constraints.MAX_NAME_LENGTH);

            entity.Property(m => m.StreetType).HasMaxLength(
                Constraints.MAX_SHORT_NAME_LENGTH).IsRequired();

            entity.Property(m => m.StreetName).HasMaxLength(
                Constraints.MAX_NAME_LENGTH).IsRequired();

            entity.Property(m => m.StreetNumber).HasMaxLength(
                Constraints.MAX_CODE_LENGTH).IsRequired();

            entity.Property(m => m.PostalCode).HasMaxLength(
                Constraints.MAX_CODE_LENGTH).IsRequired();

            entity.Property(m => m.BlockNumber).HasMaxLength(
                Constraints.MAX_CODE_LENGTH);

            entity.Property(m => m.StairNumber).HasMaxLength(
                Constraints.MAX_CODE_LENGTH);

            entity.Property(m => m.FloorNumber).HasMaxLength(
                Constraints.MAX_CODE_LENGTH);

            entity.Property(m => m.CountryId);

            entity.Property(m => m.CountyId);

            entity.Property(m => m.CreatedAtUtc).IsRequired();

            entity.Property(m => m.LastModifiedAtUtc);

            entity.HasOne(m => m.Person).WithMany(
                m => m.Addresses).HasForeignKey(
                m => m.PersonId);

            entity.HasOne(m => m.Country).WithMany(
                m => m.Addresses).HasForeignKey(
                m => m.CountryId);

            entity.HasOne(m => m.County).WithMany(
                m => m.Addresses).HasForeignKey(
                m => m.CountyId);
        }
    }
}
