using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.Service.AuthService;

namespace Notifications.API.Controllers;

[ApiController]
[Route("auth")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await auth.LoginAsync(HttpContext, request.ApiKey, cancellationToken);

        if (result == null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost("api-key")] // ТЕСТ УДАЛИТЬ
    [ProducesResponseType(typeof(ApiKeyResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateApiKey(
        [FromBody] CreateApiKeyRequest request,
        CancellationToken cancellationToken)
    {
        var key = await auth.CreateApiKeyAsync(request.Owner, request.Desc, request.CreatedBy, cancellationToken);

        return Ok(new ApiKeyResponse
        {
            Key = key
        });
    }

    [Authorize]
    [HttpGet("secure")]
    public IActionResult Secure()
    {
        return Ok(new SecureResponse
        {
            Message = "You are authorized",
            Owner = User.FindFirst("owner")?.Value,
            Desc = User.FindFirst("description")?.Value,
            CreatedAt = User.FindFirst("createdAt")?.Value,
            UpdatedAt = User.FindFirst("updatedAt")?.Value,
            CreatedBy = User.FindFirst("createdBy")?.Value
        });
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return NoContent();
    }
}