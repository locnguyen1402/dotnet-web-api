using IdentityApi.Models;

namespace IdentityApi.Services.IServices;

public interface IAuthManager
{
    ValueTask<string> CreateTokenAsync(AppUser user);
    Task AddClaimsToRoleAsync(AppRole role, string[] claims);
}