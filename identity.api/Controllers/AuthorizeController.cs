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

    [ClaimsAuthorize(ClaimsCheckType.HasAll, new string[] { PermissionsConstants.GET_USERS, PermissionsConstants.GET_USER, PermissionsConstants.GET_STAFFS })]
    [HttpGet("claims/test/has-all")]
    public IActionResult TestHasAll()
    {
        return Ok();
    }

    [ClaimsAuthorize(ClaimsCheckType.HasOne, new string[] { PermissionsConstants.GET_USER })]
    [HttpGet("claims/test/has-one")]
    public IActionResult TestHasOne()
    {
        return Ok();
    }

    [ClaimsAuthorize(ClaimsCheckType.HasAny, new string[] { PermissionsConstants.GET_USERS, PermissionsConstants.GET_USER })]
    [HttpGet("claims/test/has-any")]
    public IActionResult TestHasAny()
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