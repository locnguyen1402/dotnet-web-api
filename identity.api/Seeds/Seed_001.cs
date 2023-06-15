using IdentityApi.Constants;
using IdentityApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Seeds;

public static class Seed_001
{
    public static WebApplication Seed001(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        try
        {
            Task.Run(
                async () =>
                {
                    await InitRolesAsync(serviceProvider);
                    await InitUsersAsync(serviceProvider);
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

    private static async Task InitRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

        await CreateRoleAsync(roleManager, RoleNameConstants.ADMIN);
        await CreateRoleAsync(roleManager, RoleNameConstants.USER);
        await CreateRoleAsync(roleManager, RoleNameConstants.AGENT);
        await CreateRoleAsync(roleManager, RoleNameConstants.AGENT_ADMIN);
    }

    private static async Task InitUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        await CreateUserAsync(userManager, new("admin", "P#ssw0rd", RoleNameConstants.ADMIN));
        await CreateUserAsync(userManager, new("user", "P#ssw0rd", RoleNameConstants.USER));
        await CreateUserAsync(userManager, new("agent", "P#ssw0rd", RoleNameConstants.AGENT));
    }

    private static async Task CreateRoleAsync(RoleManager<AppRole> roleManager, string roleName)
    {
        if ((await roleManager.FindByNameAsync(roleName)) == null)
        {
            await roleManager.CreateAsync(new AppRole(roleName));
        }
    }

    private static async Task CreateUserAsync(UserManager<AppUser> userManager, UserInfo userInfo)
    {
        if ((await userManager.FindByNameAsync(userInfo.UserName)) == null)
        {
            var user = new AppUser(userInfo.UserName);

            var result = await userManager.CreateAsync(user, userInfo.Password);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = await userManager.AddToRoleAsync(user, userInfo.RoleName);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
    }

    internal class UserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public UserInfo(string userName, string password, string roleName)
        {
            UserName = userName;
            Password = password;
            RoleName = roleName;
        }
    }
}