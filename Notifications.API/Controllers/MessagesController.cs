using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.Service.MessageService;

namespace Notifications.API.Controllers;

[ApiController]
[Route("messages")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class MessagesController(IMessageService messageService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(CreateMessageResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateMessageRequest request)
    {
        var result = await messageService.CreateAsync(request);

        return Accepted(result);
    }
}