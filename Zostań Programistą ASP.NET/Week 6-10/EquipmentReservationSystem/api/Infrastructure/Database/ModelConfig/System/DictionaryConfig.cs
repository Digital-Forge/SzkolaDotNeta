using Domain.Models.System;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.ModelConfig.System
{
    public class DictionaryConfig : DatabaseModelConfig<Dictionary>
    {
        public override void ModelConfig(EntityTypeBuilder<Dictionary> builder)
        {
            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(200);

            builder.Property(e => e.Value)
                .HasColumnName("value")
                .HasMaxLength(1000);

            builder.Property(e => e.Order)
                .HasColumnName("order");

            builder.Property(e => e.Const)
                .HasColumnName("const");

            builder.Property(e => e.Active)
                .HasColumnName("active");

            builder.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion(new EnumToStringConverter<DictionaryType>())
                .HasMaxLength(100); ;
        }
    }
}
