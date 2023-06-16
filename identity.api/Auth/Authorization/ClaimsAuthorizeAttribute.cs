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
    public ClaimsAuthorizeAttribute(ClaimsCheckType checkType, string[] claims)
    {
        Policy = $"{checkType.ToString()}{string.Join(",", claims)}";
    }
}