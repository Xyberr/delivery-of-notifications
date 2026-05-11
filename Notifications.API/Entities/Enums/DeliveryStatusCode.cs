using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Entities.Enums;

public enum DeliveryStatusCode
{
    [Display(Name = "В очереди", Description = "Сообщение добавлено в очередь")]
    Queued = 1,

    [Display(Name = "В обработке", Description = "Сообщение обрабатывается")]
    Pending = 2,

    [Display(Name = "Отправлено", Description = "Сообщение успешно доставлено")]
    Delivered = 3,
    
    [Display(Name = "Ошибка", Description = "Исчерпан лимит попыток")]
    Failed = 4,
    
    [Display(Name = "Не поддерживается", Description = "Не поддерживается")]
    NotSupported = 9999
}