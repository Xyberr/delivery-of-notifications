using Microsoft.AspNetCore.Mvc;
using Notifications.API.Service.AuthService;


namespace Notifications.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Auth()
    {
        var token = await _authService.GenerateAndSaveToken();
        return Ok(token);
    }
}