using Microsoft.AspNetCore.Identity;
namespace IdentityApi.Models;
public class AppUserLogin : IdentityUserLogin<Guid>
{
    public virtual AppUser User { get; set; } = null!;
}
