using Domain.Interfaces.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig
{
    public interface IDatabaseConfig
    {
        void Config(ModelBuilder builder);
    }

    public abstract class DatabaseModelConfig<TEntity> : IDatabaseConfig
        where TEntity : class
    {
        public abstract void ModelConfig(EntityTypeBuilder<TEntity> builder);

        public void Config(ModelBuilder builder)
        {
            var entityBuilder = builder.Entity<TEntity>();
            ModelConfig(entityBuilder);

            if (typeof(TEntity).GetInterface(nameof(IAuditableEntity)) != null)
                AuditableEntityConfig.DefaultAuditableEntityConfig(entityBuilder);
        }

    }
}
