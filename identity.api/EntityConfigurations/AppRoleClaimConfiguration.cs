using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using IdentityApi.Models;

namespace IdentityApi.EntityConfigurations;
public class AppRoleClaimConfiguration : IEntityTypeConfiguration<AppRoleClaim>
{
    public void Configure(EntityTypeBuilder<AppRoleClaim> builder)
    {
        builder.HasOne(t => t.Role)
            .WithMany(t => t.RoleClaims)
            .HasForeignKey(t => t.RoleId)
            .IsRequired();
    }
}