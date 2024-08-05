using Domain.Models.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.Business
{
    public class DepartmentConfig : DatabaseModelConfig<Department>
    {
        public override void ModelConfig(EntityTypeBuilder<Department> builder)
        {
            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(200);

            builder.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(3000);
        }
    }
}
