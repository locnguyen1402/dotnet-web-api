using AutoMapper;
using IdentityApi.Auth.Authorization;
using IdentityApi.Constants;
using IdentityApi.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers;
public class AuthorizeController : BaseController
{
    public AuthorizeController(
        ILogger<AccountController> logger,
        IMapper mapper,
        IIdentityService identityService
    ) : base(logger, mapper, identityService)
    {
    }

    [ClaimsAuthorize(new string[] { PermissionsConstants.GET_USER, PermissionsConstants.GET_USERS }, CheckType = ClaimsCheckType.HasAll)]
    [HttpGet("claims/test")]
    public IActionResult TestClaims()
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("claims/me")]
    public IActionResult GetMyClaims()
    {
        return Ok(new
        {
            claims = _identityService.PermissionClaims,
        });
    }

    [Authorize]
    [HttpGet("user")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult RoleUserGet()
    {
        return Ok(RoleNameConstants.USER);
    }

    [Authorize(Roles = RoleNameConstants.ADMIN)]
    [HttpGet("admin")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult RoleAdminGet()
    {
        return Ok(RoleNameConstants.ADMIN);
    }

    [Authorize(Roles = $"{RoleNameConstants.AGENT},{RoleNameConstants.ADMIN}")]
    [HttpGet("agent")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult RoleAgentGet()
    {
        return Ok(RoleNameConstants.AGENT);
    }
}