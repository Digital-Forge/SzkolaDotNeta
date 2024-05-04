using Domain.Interfaces.Models;
using Domain.Models.Business;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.ModelConfig.Business
{
    public class ItemInstanceConfig : DatabaseModelConfig<ItemInstance>
    {
        public override void ModelConfig(EntityTypeBuilder<ItemInstance> builder)
        {
            builder.Property(e => e.SerialNumber)
                .HasColumnName("serial_number")
                .HasMaxLength(200);

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion(new EnumToStringConverter<ItemInstanceStatus>())
                .HasMaxLength(20);

            builder.Property(e => e.AddedDate)
                .HasColumnName("added_date");

            builder.Property(e => e.WithdrawalDate)
                .HasColumnName("withdrawal_date");

            //Relations
            builder.HasOne(r => r.Item)
                .WithMany(r => r.Instances)
                .HasForeignKey(r => r.ItemId);
        }
    }
}
