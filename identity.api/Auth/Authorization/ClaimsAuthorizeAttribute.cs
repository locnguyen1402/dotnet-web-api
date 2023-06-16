using Microsoft.AspNetCore.Authorization;

namespace IdentityApi.Auth.Authorization;

public enum ClaimsCheckType
{
    HasOne,
    HasAny,
    HasAll
}

public class ClaimsAuthorizeAttribute : AuthorizeAttribute
{
    public ClaimsCheckType CheckType { get; set; } = ClaimsCheckType.HasOne;
    public ClaimsAuthorizeAttribute(string[] claims)
    {
        Policy = $"{CheckType.ToString()}{string.Join(",", claims)}";
    }
}