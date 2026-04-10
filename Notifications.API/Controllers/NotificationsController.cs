using Microsoft.AspNetCore.Mvc;
using Notifications.Contracts.DTO.Responses;
using Notifications.Contracts.Requests;
using Notifications.Core.Interfaces;

namespace Notifications.API.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }
    
    [HttpPost("enqueue-message")]
    [ProducesResponseType(typeof(CreateNotificationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateNotificationResponse>> EnqueueMessage(
        [FromBody] CreateNotificationRequest request)
    {
        // Автоматическая валидация через [ApiController]
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = await _service.CreateAsync(request);

        return Ok(new CreateNotificationResponse
        {
            Id = id,
            Status = "Pending"
        });
    }
}