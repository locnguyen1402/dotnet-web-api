using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using IdentityApi.Models;

namespace IdentityApi.EntityConfigurations;
public class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(t => t.UserRoles)
            .HasForeignKey(t => t.UserId)
            .IsRequired();

        builder.HasOne(t => t.Role)
            .WithMany(t => t.UserRoles)
            .HasForeignKey(t => t.RoleId)
            .IsRequired();
    }
}