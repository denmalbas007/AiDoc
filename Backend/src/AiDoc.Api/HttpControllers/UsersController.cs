using System.Threading.Tasks;
using AiDoc.Api.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiDoc.Api.HttpControllers;

[ApiController]
[Route("api/v1/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
        => _usersService = usersService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await _usersService.GetCurrentUserAsync(HttpContext.RequestAborted);
        return Ok(result);
    }
    
    [HttpGet("content")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUserContent()
    {
        var result = await _usersService.GetCurrentUserContentAsync(HttpContext.RequestAborted);
        return Ok(result);
    }
}