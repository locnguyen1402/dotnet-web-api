using System.Security.Claims;
using IdentityApi.Constants;
using IdentityApi.Services.IServices;

namespace IdentityApi.Services;

public class IdentityService : IIdentityService
{
    private HttpContext _httpContext;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor?.HttpContext ?? throw new NotImplementedException();
    }
    public bool IsAuthenticated => _httpContext.User!.Identity!.IsAuthenticated;

    public List<string> PermissionClaims
    {
        get
        {
            return _httpContext.User.FindAll(SecurityClaimTypes.Permission).Select(c => c.Value).ToList();
        }
    }
}