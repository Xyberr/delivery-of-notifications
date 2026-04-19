using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.Service.AuthService;


namespace Notifications.API.Controllers;

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
        var apiKey = await _auth.ValidateApiKey(request.ApiKey);

        if (apiKey == null)
            return Unauthorized();

        var claims = new List<Claim>
        {
            new Claim("owner", apiKey.Owner)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

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