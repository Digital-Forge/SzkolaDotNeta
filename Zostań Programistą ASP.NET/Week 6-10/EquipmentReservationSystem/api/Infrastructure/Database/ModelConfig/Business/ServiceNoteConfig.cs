using Domain.Models.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.Business
{
    public class ServiceNoteConfig : DatabaseModelConfig<ServiceNote>
    {
        public override void ModelConfig(EntityTypeBuilder<ServiceNote> builder)
        {
            builder.Property(p => p.ItemInstanceId)
                .HasColumnName("item_instance_id");

            builder.Property(p => p.Note)
                .HasColumnName("note")
                .HasMaxLength(3000);
        }
    }
}
