using Domain.Models.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelConfig.System
{
    public class RefreshTokenConfig : DatabaseModelConfig<RefreshToken>
    {
        public override void ModelConfig(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Token)
                .HasColumnName("token")
                .IsUnicode();

            builder.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsUnicode();

            builder.Property(e => e.LifeTime)
                .HasColumnName("life_time");
        }
    }
}
