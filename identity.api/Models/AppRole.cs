using Microsoft.AspNetCore.Identity;
using IdentityApi.Models.IModels;

namespace IdentityApi.Models;

public class AppRole : IdentityRole<Guid>, IBaseEntity
{
    public ICollection<AppRoleClaim> RoleClaims { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
    public AppRole() : base()
    {
        RoleClaims = new List<AppRoleClaim>();
        UserRoles = new List<AppUserRole>();
    }
    public AppRole(string roleName) : base(roleName)
    {
        RoleClaims = new List<AppRoleClaim>();
        UserRoles = new List<AppUserRole>();
    }
}