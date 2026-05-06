using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Entities.Enums;

public enum DeliveryStatusCode
{
    [Display(Name = "В очереди", Description = "Сообщение добавлено в очередь")]
    Queued = 0,

    [Display(Name = "В обработке", Description = "Сообщение обрабатывается")]
    Pending = 1,

    [Display(Name = "Отправлено", Description = "Сообщение успешно доставлено")]
    Delivered = 2,

    [Display(Name = "Ошибка", Description = "Ошибка при отправке")]
    Failed = 3
}