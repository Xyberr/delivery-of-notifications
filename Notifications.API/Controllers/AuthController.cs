using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.Service.AuthService;

namespace Notifications.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (principal, data) = await auth.Authenticate(request.ApiKey);

        if (principal == null || data == null)
            return Unauthorized();

        await HttpContext.SignInAsync(principal);
        
        return Ok(data);
    }

    [HttpPost("api-key")] // ТЕСТ, УДАЛИТЬ
    public async Task<IActionResult> CreateApiKey([FromBody] string owner, string desc)
    {
        var key = await auth.CreateApiKey(owner, desc);
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
            owner,
            desc = User.FindFirst("description")?.Value,
            CreatedAt = User.FindFirst("createdAt")?.Value
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}