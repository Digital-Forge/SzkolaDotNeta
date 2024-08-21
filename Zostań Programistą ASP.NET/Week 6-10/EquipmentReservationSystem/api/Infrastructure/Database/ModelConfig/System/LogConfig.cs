using Domain.Models.System;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.ModelConfig.System
{
    public class LogConfig : DatabaseModelConfig<Log>
    {
        public override void ModelConfig(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id");


            builder.Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder.Property(x => x.Message)
                .HasColumnName("message");

            builder.Property(x => x.Source)
                .HasColumnName("source");

            builder.Property(x => x.StackTrace)
                .HasColumnName("stack_trace");

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .HasConversion(new EnumToStringConverter<LogType>())
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.Date)
                .HasColumnName("date");

            builder.Property(x => x.UserId)
                .HasColumnName("user_Id");

            builder.Property(x => x.ParentLogId)
                .HasColumnName("parent_log_id");
        }
    }
}
