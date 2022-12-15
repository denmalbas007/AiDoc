using System.Threading.Tasks;
using AiDoc.Api.Services.Authorization;
using AiDoc.Api.Services.Authorization.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AiDoc.Api.HttpControllers;

[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public AuthController(IAuthorizationService authorizationService)
        => _authorizationService = authorizationService;

    [HttpPost]
    public async Task<IActionResult> AuthorizeUser(AuthorizeUserRequest request)
    {
        var result = await _authorizationService.AuthorizeUserAsync(request, HttpContext.RequestAborted);
        return Ok(result);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterNewUser(RegisterNewUserRequest request)
    {
        var result = await _authorizationService.RegisterNewUserAsync(request, HttpContext.RequestAborted);
        return Ok(result);
    }
}