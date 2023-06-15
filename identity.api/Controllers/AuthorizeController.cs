using System.Net;
using AutoMapper;
using IdentityApi.Constants;
using IdentityApi.Controllers.Requests;
using IdentityApi.Models;
using IdentityApi.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IdentityApi.Controllers;
public class AuthorizeController : BaseController
{

    public AuthorizeController(
        ILogger<AccountController> logger,
        IMapper mapper
    ) : base(logger, mapper)
    {
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