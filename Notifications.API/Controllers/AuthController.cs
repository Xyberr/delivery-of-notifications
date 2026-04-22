using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.Service.AuthService;


namespace Notifications.API.Controllers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.Service.AuthService;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var principal = await _auth.Authenticate(request.ApiKey);

        if (principal == null)
            return Unauthorized();

        await HttpContext.SignInAsync(principal);

        return Ok();
    }

    [HttpPost("api-key")]
    public async Task<IActionResult> CreateApiKey([FromBody] string owner)
    {
        var key = await _auth.CreateApiKey(owner);
        return Ok(new { key });
    }

    [Authorize]
    [HttpGet("secure")]
    public IActionResult Secure()
    {
        var owner = User.FindFirst("owner")?.Value;

        return Ok(new
        {
            message = "You are authorized",
            owner
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}