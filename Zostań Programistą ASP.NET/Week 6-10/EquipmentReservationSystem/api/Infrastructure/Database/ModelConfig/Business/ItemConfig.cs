using Domain.Models.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.Business
{
    public class ItemConfig : DatabaseModelConfig<Item>
    {
        public override void ModelConfig(EntityTypeBuilder<Item> builder)
        {
            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(300);

            builder.Property(e => e.Description)
                .HasColumnName("description");

            builder.Property(e => e.Images)
                .HasColumnName("image");

            builder.Property(e => e.AddictionFiles)
                .HasColumnName("addiction_files");
        }
    }
}
