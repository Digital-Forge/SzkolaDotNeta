using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.Business
{
    public class UserDataConfig : DatabaseModelConfig<UserData>
    {
        public override void ModelConfig(EntityTypeBuilder<UserData> builder)
        {
        }
    }
}
