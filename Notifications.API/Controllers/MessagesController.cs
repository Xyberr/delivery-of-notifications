using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.API.DTO.Requests;
using Notifications.API.DTO.Responses;
using Notifications.API.Service.MessageService;

namespace Notifications.API.Controllers;

[ApiController]
[Route("messages")]
[Authorize]
public class MessagesController(IMessageService service) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateMessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateMessageRequest request)
    {
        try
        {
            var result = await service.CreateAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long id)
    {
        var result = await service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<MessageResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {
        var result = await service.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var deleted = await service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}