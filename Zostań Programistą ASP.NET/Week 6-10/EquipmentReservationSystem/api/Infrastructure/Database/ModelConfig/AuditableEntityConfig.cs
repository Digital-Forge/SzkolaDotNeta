using Domain.Interfaces.Models;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.ModelConfig
{
    public static class AuditableEntityConfig
    {
        public static void DefaultAuditableEntityConfig<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class
        {
            builder.HasKey(e => (e as IAuditableEntity).Id);
            builder.Property(e => (e as IAuditableEntity).Id)
                .HasColumnName("id");

            builder.Property(e => (e as IAuditableEntity).CreateBy)
                .HasColumnName("create_by");

            builder.Property(e => (e as IAuditableEntity).CreateTime)
                .HasColumnName("create_time");

            builder.Property(e => (e as IAuditableEntity).UpdateBy)
                .HasColumnName("update_by");

            builder.Property(e => (e as IAuditableEntity).UpdateTime)
                .HasColumnName("update_time");

            builder.Property(e => (e as IAuditableEntity).EntityStatus)
                .HasColumnName("entity_status")
                .HasConversion(new EnumToStringConverter<EntityStatus>())
                .HasMaxLength(20);

            builder.Property(e => (e as IAuditableEntity).Active)
                .HasColumnName("active");
        }
    }
}
