using Domain.Models.Business.MiddleTabs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.Business.MiddleTabs
{
    public class ItemToDepartmentConfig : DatabaseModelConfig<ItemToDepartment>
    {
        public override void ModelConfig(EntityTypeBuilder<ItemToDepartment> builder)
        {
            builder.HasKey(k => new { k.ItemId, k.DepartmentId });

            builder.HasOne(r => r.Item)
                .WithMany(r => r.Departments)
                .HasForeignKey(r => r.ItemId);

            builder.HasOne(r => r.Department)
                .WithMany(r => r.Items)
                .HasForeignKey(r => r.DepartmentId);
        }
    }
}
