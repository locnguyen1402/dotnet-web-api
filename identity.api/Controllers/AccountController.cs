using System.Net;
using System.Reflection;
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
    public AccountController(
        ILogger<AccountController> logger,
        IMapper mapper,
        UserManager<AppUser> userManager,
        IAuthManager authManager,
        IIdentityService identityService
    ) : base(logger, mapper, identityService)
    {
        _userManager = userManager;
        _authManager = authManager;
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

}