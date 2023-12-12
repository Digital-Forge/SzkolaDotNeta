using Domain.Models.Business.MiddleTabs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.Business.MiddleTabs
{
    public class UserToDepartmentConfig : DatabaseModelConfig<UserToDepartment>
    {
        public override void ModelConfig(EntityTypeBuilder<UserToDepartment> builder)
        {
            builder.HasKey(k => new { k.UserId, k.DepartmentId });

            builder.HasOne(r => r.User)
                .WithMany(r => r.Departments)
                .HasForeignKey(r => r.UserId);

            builder.HasOne(r => r.Department)
                .WithMany(r => r.Users)
                .HasForeignKey(r => r.DepartmentId);
        }
    }
}
