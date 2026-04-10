using Microsoft.AspNetCore.Mvc;
using Notifications.Contracts.Requests;
using Notifications.Infrastructure.Services;

namespace Notifications.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => (await _service.Get(id)) is { } user
            ? Ok(user)
            : NotFound();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        => Ok(await _service.Create(request));
    

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
        => await _service.Update(id, request)
            ? NoContent()
            : NotFound();

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
        => await _service.Delete(id)
            ? NoContent()
            : NotFound();
}