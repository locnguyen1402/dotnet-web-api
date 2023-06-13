using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models;
public class AppUserClaim : IdentityUserClaim<Guid>
{
    public virtual AppUser User { get; private set; } = null!;
}
