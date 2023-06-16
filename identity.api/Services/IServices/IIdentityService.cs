using System.Security.Claims;

namespace IdentityApi.Services.IServices;

public interface IIdentityService
{
    List<string> PermissionClaims { get; }
    bool IsAuthenticated { get; }
}