using IdentityApi.Constants;
using IdentityApi.Data;
using IdentityApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Seeds;

public static class Seed_001
{
    public static WebApplication Seed001(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        try
        {
            Task.Run(
                async () =>
                {
                    await CreateRoleAsync(roleManager, RoleNameConstants.ADMIN);
                    await CreateRoleAsync(roleManager, RoleNameConstants.USER);
                    await CreateRoleAsync(roleManager, RoleNameConstants.AGENT);
                })
             .GetAwaiter()
             .GetResult();
        }
        catch (Exception)
        {

            throw;
        }

        return application;
    }

    private static async Task CreateRoleAsync(RoleManager<AppRole> roleManager, string roleName)
    {
        if ((await roleManager.FindByNameAsync(roleName)) == null)
        {
            await roleManager.CreateAsync(new AppRole(roleName));
        }
    }
}