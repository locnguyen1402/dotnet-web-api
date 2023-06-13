using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using IdentityApi.Models;

namespace IdentityApi.EntityConfigurations;
public class AppUserTokenConfiguration : IEntityTypeConfiguration<AppUserToken>
{
    public void Configure(EntityTypeBuilder<AppUserToken> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(t => t.UserTokens)
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}