using Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public static class EntityMapperCore
    {
        public static EntityTypeBuilder<TEntity> AddMappings<TEntity, TPk>(
            ModelBuilder modelBuilder,
            bool makeAutoIncrement)
            where TEntity : EntityBase<TPk>
            where TPk : IEquatable<TPk>
        {
            var entity = modelBuilder.Entity<TEntity>();
            entity.HasKey(m => m.Id);

            var propBuilder = entity.Property(
                m => m.Id).IsRequired();

            if (!makeAutoIncrement)
            {
                propBuilder.ValueGeneratedNever();
            }

            return entity;
        }
    }
}
