using Domain.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.System
{
    public class DataFileConfig : DatabaseModelConfig<DataFile>
    {
        public override void ModelConfig(EntityTypeBuilder<DataFile> builder)
        {
            builder.Property(e => e.OriginName)
                .HasColumnName("origin_name")
                .HasMaxLength(500);

            builder.Property(e => e.Format)
                .HasColumnName("format")
                .HasMaxLength(10);
        }
    }
}
