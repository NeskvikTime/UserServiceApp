using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id)
              .IsClustered(false);

        builder.HasIndex(r => r.Id)
            .IsUnique();

        builder.Property(r => r.Value)
            .HasMaxLength(200);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);

        builder.Property(r => r.ExpiresOnUtc)
            .IsRequired();
    }
}
