using Microsoft.EntityFrameworkCore.Metadata.Builders;

using IdentityApi.Models;

namespace IdentityApi.EntityConfigurations;

public class AppRoleConfiguration : BaseEntityConfiguration<AppRole>
{
    public override void Configure(EntityTypeBuilder<AppRole> builder)
    {
        base.Configure(builder);

        builder.HasIndex(e => e.NormalizedName)
            .IsUnique();

        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.NormalizedName)
            .HasMaxLength(50)
            .IsRequired();
    }
}