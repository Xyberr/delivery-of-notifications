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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await auth.LoginAsync(HttpContext, request.ApiKey);

        if (result == null)
            return Unauthorized();

        return Ok(result);
    }
    
    [HttpPost("api-key")] // ТЕСТ, УДАЛИТЬ
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateApiKey([FromBody]string owner, string desc, string createBy)
    {
        var key = await auth.CreateApiKey(owner, desc, createBy);
        return Ok(new { key });
    }

    [Authorize]
    [HttpGet("secure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Secure()
    {
        var owner = User.FindFirst("owner")?.Value;

        return Ok(new
        {
            message = "You are authorized",
            owner,
            desc = User.FindFirst("description")?.Value,
            CreatedAt = User.FindFirst("createdAt")?.Value,
            UpdatedAt = User.FindFirst("updatedAt")?.Value,
            CreatedBy = User.FindFirst("createdBy")?.Value,
        });
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}