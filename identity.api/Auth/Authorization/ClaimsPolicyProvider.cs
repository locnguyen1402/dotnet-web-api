using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace IdentityApi.Auth.Authorization;

public class ClaimsPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; } = null!;
    public ClaimsPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return FallbackPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return FallbackPolicyProvider.GetFallbackPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        AuthorizationPolicyBuilder builder = new();
        AuthorizationPolicy? policy = null;

        if (policyName.StartsWith(ClaimsCheckType.HasOne.ToString()))
        {
            builder.AddRequirements(
                new ClaimsAuthorizeRequirement
                {
                    CheckType = ClaimsCheckType.HasOne,
                    ClaimValues = GetClaims(ClaimsCheckType.HasOne, policyName)
                }
            );
            policy = builder.Build();
        }

        if (policyName.StartsWith(ClaimsCheckType.HasAny.ToString()))
        {
            builder.AddRequirements(
                new ClaimsAuthorizeRequirement
                {
                    CheckType = ClaimsCheckType.HasAny,
                    ClaimValues = GetClaims(ClaimsCheckType.HasAny, policyName)
                }
            );
            policy = builder.Build();
        }

        if (policyName.StartsWith(ClaimsCheckType.HasAll.ToString()))
        {
            builder.AddRequirements(
                new ClaimsAuthorizeRequirement
                {
                    CheckType = ClaimsCheckType.HasAll,
                    ClaimValues = GetClaims(ClaimsCheckType.HasAll, policyName)
                }
            );
            policy = builder.Build();
        }

        if (policy != null)
        {
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    private string[] GetClaims(ClaimsCheckType checkType, string policyString)
    {
        return policyString.Substring(checkType.ToString().Length).Split(",");
    }
}