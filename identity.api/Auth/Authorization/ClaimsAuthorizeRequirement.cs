using Microsoft.AspNetCore.Authorization;

namespace IdentityApi.Auth.Authorization;

public class ClaimsAuthorizeRequirement : IAuthorizationRequirement
{
    public ClaimsCheckType CheckType { get; set; }
    public string[] ClaimValues { get; set; } = new string[] { };
}