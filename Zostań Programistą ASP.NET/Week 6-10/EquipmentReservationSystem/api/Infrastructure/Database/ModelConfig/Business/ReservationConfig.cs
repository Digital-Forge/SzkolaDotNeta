using Domain.Models.Business;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database.ModelConfig.Business
{
    public class ReservationConfig : DatabaseModelConfig<Reservation>
    {
        public override void ModelConfig(EntityTypeBuilder<Reservation> builder)
        {
            builder.Property(e => e.From)
                .HasColumnName("from");
            
            builder.Property(e => e.To)
                .HasColumnName("to");

            builder.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion(new EnumToStringConverter<ReservationStatus>())
                .HasMaxLength(30);

            //Relations
            builder.HasOne(r => r.ItemInstance)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.ItemInstanceId);

            builder.HasOne(r => r.User)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.UserId);
        }
    }
}
