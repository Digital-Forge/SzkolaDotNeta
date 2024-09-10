using Domain.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.System
{
    public class SerilogEntryConfig : DatabaseModelConfig<SerilogEntry>
    {
        public override void ModelConfig(EntityTypeBuilder<SerilogEntry> builder)
        {
            builder.ToTable("Logs", "oss");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Level)
                .HasMaxLength(16);

            builder.Property(p => p.TimeStamp)
                .HasColumnType("datetime");

            builder.Property(p => p.Exception)
                .IsRequired(false);

            builder.Property(p => p.Properties)
                .IsRequired(false);

            builder.Property(p => p.LogEvent)
                .IsRequired(false);
        }
    }
}