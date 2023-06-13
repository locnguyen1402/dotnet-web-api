using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using IdentityApi.Models;

namespace IdentityApi.EntityConfigurations;
public class AppUserClaimConfiguration : IEntityTypeConfiguration<AppUserClaim>
{
    public void Configure(EntityTypeBuilder<AppUserClaim> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(t => t.UserClaims)
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}