using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models;
public class AppUserToken : IdentityUserToken<Guid>
{
    public virtual AppUser User { get; set; } = null!;
}
