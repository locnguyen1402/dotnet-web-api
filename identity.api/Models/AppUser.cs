using IdentityApi.Models.IModels;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models;

public class AppUser : IdentityUser<Guid>, IBaseEntity
{
    public string? FirstName { get; private set; } = string.Empty;
    public string? LastName { get; private set; } = string.Empty;
    public ICollection<AppUserClaim> UserClaims { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
    public ICollection<AppUserLogin> UserLogins { get; set; }
    public ICollection<AppUserToken> UserTokens { get; set; }
    public AppUser(string userName) : base(userName)
    {
        UserClaims = new List<AppUserClaim>();
        UserRoles = new List<AppUserRole>();
        UserLogins = new List<AppUserLogin>();
        UserTokens = new List<AppUserToken>();
    }

    public AppUser(string userName, string firstName, string lastName) : this(userName)
    {
        FirstName = firstName;
        LastName = lastName;

        UserClaims = new List<AppUserClaim>();
        UserRoles = new List<AppUserRole>();
        UserLogins = new List<AppUserLogin>();
        UserTokens = new List<AppUserToken>();
    }

    public void UpdateUserInfo(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}