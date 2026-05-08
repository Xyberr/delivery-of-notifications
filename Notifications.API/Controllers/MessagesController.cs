using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.Service.MessageService;

namespace Notifications.API.Controllers;

[ApiController]
[Route("messages")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[Authorize]
public class MessagesController(IMessageService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateMessageResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(CreateMessageRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(result.Data);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ICollection<MessageResponse>), StatusCodes.Status200OK)]    
    public async Task<IActionResult> GetList(CancellationToken cancellationToken)
    {
        var result = await service.GetListAsync(cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(id, cancellationToken);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}