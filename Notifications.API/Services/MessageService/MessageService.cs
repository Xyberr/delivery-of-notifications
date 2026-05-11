using Notifications.API.Persistence;
using MassTransit;
using Notifications.API.Services.DeliveryStatusProvider;

namespace Notifications.API.Service.MessageService;

public partial class MessageService(AppDbContext db, ILogger<MessageService> logger, IDeliveryStatusProvider deliveryStatusProvider) : IMessageService;