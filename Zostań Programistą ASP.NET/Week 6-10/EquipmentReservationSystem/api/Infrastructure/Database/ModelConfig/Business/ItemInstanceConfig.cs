using Domain.Models.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.Business
{
    public class ItemInstanceConfig : DatabaseModelConfig<ItemInstance>
    {
        public override void ModelConfig(EntityTypeBuilder<ItemInstance> builder)
        {
            builder.Property(e => e.SerialNumber)
                .HasColumnName("serial_number")
                .HasMaxLength(200);

            builder.Property(e => e.Starus)
                .HasColumnName("status");

            builder.Property(e => e.DurabilityStatus)
                .HasColumnName("durability_status");

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
