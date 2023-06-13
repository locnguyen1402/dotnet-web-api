
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using IdentityApi.Models;

namespace IdentityApi.EntityConfigurations;

public class AppUserConfiguration : BaseEntityConfiguration<AppUser>
{
    public override void Configure(EntityTypeBuilder<AppUser> builder)
    {
        base.Configure(builder);

        builder.HasIndex(e => e.NormalizedUserName)
            .IsUnique();

        builder.Property(e => e.NormalizedUserName)
            .HasMaxLength(50);

        builder.Property(e => e.UserName)
            .HasMaxLength(50);

        builder.Property(e => e.FirstName)
            .HasMaxLength(50);

        builder.Property(e => e.LastName)
            .HasMaxLength(50);
    }
}