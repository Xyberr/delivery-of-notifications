using Notifications.API.Persistence;

namespace Notifications.API.Service.MessageService;

public partial class MessageService(AppDbContext db, ILogger<MessageService> logger) : IMessageService;