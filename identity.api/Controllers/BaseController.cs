using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly ILogger<BaseController> _logger;
    protected readonly private IMapper _mapper;
    protected BaseController(ILogger<BaseController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }
}