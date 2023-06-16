using AutoMapper;
using IdentityApi.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly IIdentityService _identityService;
    protected readonly ILogger<BaseController> _logger;
    protected readonly private IMapper _mapper;
    protected BaseController(
        ILogger<BaseController> logger,
        IMapper mapper,
        IIdentityService identityService
    )
    {
        _logger = logger;
        _mapper = mapper;
        _identityService = identityService;
    }
}