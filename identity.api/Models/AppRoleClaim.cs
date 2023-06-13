using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models;

public class AppRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual AppRole Role { get; set; } = null!;
}
