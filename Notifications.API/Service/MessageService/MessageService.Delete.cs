using Microsoft.EntityFrameworkCore;

namespace Notifications.API.Service.MessageService;

public partial class MessageService
{
    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var affected = await db.Messages
            .Where(message => message.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return affected > 0;
    }
}