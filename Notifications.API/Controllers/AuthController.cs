using Microsoft.AspNetCore.Mvc;
using Notifications.Contracts.Requests;
using Notifications.Infrastructure.Services.Auth;

namespace Notifications.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }
    

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        var token = await _auth.Login(request.Email, request.Password);
        return Ok(new { token });
    }
}