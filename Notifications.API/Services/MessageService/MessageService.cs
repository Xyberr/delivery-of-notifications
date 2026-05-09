using Notifications.API.Persistence;
using MassTransit;

namespace Notifications.API.Service.MessageService;

public partial class MessageService(AppDbContext db, ILogger<MessageService> logger, IPublishEndpoint publish) : IMessageService;