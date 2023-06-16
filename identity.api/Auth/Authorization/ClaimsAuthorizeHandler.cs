using IdentityApi.Constants;
using IdentityApi.Services.IServices;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApi.Auth.Authorization;

public class ClaimsAuthorizeHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User == null)
        {
            context.Fail();

            return Task.CompletedTask;
        }

        var assignedClaims = context.User.FindAll(SecurityClaimTypes.Permission).Select(c => c.Value).ToList();

        var pendingRequirements = context.PendingRequirements.ToList();

        foreach (var requirement in pendingRequirements)
        {
            if (requirement is ClaimsAuthorizeRequirement)
            {
                var r = (ClaimsAuthorizeRequirement)requirement;
                var claims = r.ClaimValues;
                var isValid = false;

                if (!claims.Any())
                {
                    isValid = true;
                }
                else if (r.CheckType == ClaimsCheckType.HasOne)
                {
                    isValid = assignedClaims.Any(p => p == claims[0]);
                }
                else if (r.CheckType == ClaimsCheckType.HasAll)
                {
                    isValid = claims.All(c => assignedClaims.Any(p => p == c));
                }
                else if (r.CheckType == ClaimsCheckType.HasAny)
                {
                    isValid = claims.Any(c => assignedClaims.Any(p => p == c));
                }

                if (isValid)
                {
                    context.Succeed(r);
                }
            }
        }
        return Task.CompletedTask;

    }
}