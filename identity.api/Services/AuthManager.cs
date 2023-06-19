using Microsoft.AspNetCore.Identity;
using IdentityApi.Models;
using IdentityApi.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using IdentityApi.Settings;
using IdentityApi.Constants;
using Microsoft.EntityFrameworkCore;
using IdentityApi.Data;

namespace IdentityApi.Services;

public class AuthManager : IAuthManager
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    public AuthManager(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IConfiguration configuration,
        AppDbContext db
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _db = db;
    }
    public async ValueTask<string> CreateTokenAsync(AppUser user)
    {
        var claims = await GetClaims(user);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = GenerateTokenOptions(GetSigningCredentials(), claims);
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSetting>()!;
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private SecurityTokenDescriptor GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSetting>()!;
        var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.LifetimeInMinutes));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = expiration,
            SigningCredentials = signingCredentials,
            Subject = new ClaimsIdentity(claims),
            Issuer = jwtSettings.Issuer,
        };


        return tokenDescriptor;
    }

    private async Task<List<Claim>> GetClaims(AppUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(SecurityClaimTypes.Subject, user.Id.ToString()),
        };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(SecurityClaimTypes.Role, role));
        }

        var roleClaims = await _roleManager.Roles
                            .AsNoTracking()
                            .Include(r => r.RoleClaims)
                            .Include(r => r.UserRoles)
                            .Where(r => r.UserRoles.Any(t => t.UserId == user.Id))
                            .SelectMany(r => r.RoleClaims)
                            .Where(rl => rl.ClaimType == SecurityClaimTypes.Permission)
                            .Select(r => r.ClaimValue)
                            .Distinct()
                            .ToListAsync();

        foreach (var claim in roleClaims)
        {
            claims.Add(new Claim(SecurityClaimTypes.Permission, claim!));
        }

        return claims;
    }

    public async Task AddClaimsToRoleAsync(AppRole role, string[] claimValues)
    {
        var claims = new List<AppRoleClaim>();

        foreach (var claim in claimValues.Distinct())
        {
            claims.Add(new AppRoleClaim
            {
                RoleId = role.Id,
                ClaimType = SecurityClaimTypes.Permission,
                ClaimValue = claim
            });
        }

        _db.RoleClaims.AddRange(claims);
        await _db.SaveChangesAsync();
    }
}