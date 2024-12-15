using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UserServiceApp.Domain;

namespace UserServiceApp.Infrastructure.Persistance.Configurations;
internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasIndex(r => r.Id)
            .IsUnique()
            .IsClustered(false);

        builder.Property(r => r.Token).HasMaxLength(200);

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);


    }
}
