using System.Net;
using AutoMapper;
using IdentityApi.Constants;
using IdentityApi.Controllers.Requests;
using IdentityApi.Models;
using IdentityApi.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers;
public class AccountController : BaseController
{
    private IAuthManager _authManager;
    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;
    public AccountController(
        ILogger<AccountController> logger,
        IMapper mapper,
        UserManager<AppUser> userManager,
        IAuthManager authManager,
        IIdentityService identityService,
        RoleManager<AppRole> roleManager
    ) : base(logger, mapper, identityService)
    {
        _userManager = userManager;
        _authManager = authManager;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = new AppUser(registerRequest.Username);

            user.UpdateUserInfo(registerRequest.FirstName ?? string.Empty, registerRequest.LastName ?? string.Empty);

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, RoleNameConstants.USER);

            return Accepted();

        }
        catch (Exception)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByNameAsync(loginRequest.Username);

        if (user == null)
        {
            return BadRequest("Invalid username or password !");
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

        if (!isValidPassword)
        {
            return BadRequest("Invalid username or password !");
        }

        var token = await _authManager.CreateTokenAsync(user);

        return Ok(new { AccessToken = token });
    }

    [HttpGet("permissions")]
    public IActionResult GetPermissions()
    {

        return Ok(PermissionsConstants.GetPermissionList());
    }

    [HttpGet("permissions/me")]
    [Authorize]
    public IActionResult GetMyPermissions()
    {

        return Ok(_identityService.PermissionClaims);
    }

    [Authorize(Roles = RoleNameConstants.ADMIN)]
    [HttpPost("claims/assign")]
    public async Task<IActionResult> AssignClaims([FromBody] AssignRoleClaimsRequest request)
    {
        var role = await _roleManager.FindByNameAsync(request.RoleName.ToString());

        if (role == null)
        {
            return BadRequest("Role not found");
        }

        var perms = PermissionsConstants.GetPermissionList();

        var isValidPerms = request.Claims.All(p => perms.Any(c => c.Value == p));

        if (!isValidPerms)
        {
            return BadRequest("Permission not found");
        }

        await _authManager.AddClaimsToRoleAsync(role, request.Claims);

        return Ok();
    }
}