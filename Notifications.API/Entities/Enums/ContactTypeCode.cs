using System.ComponentModel.DataAnnotations;

namespace Notifications.API.Entities.Enums;

public enum ContactTypeCode
{
    [Display(Name = "Email", Description = "Адрес электронной почты")]
    Email = 1,
    
    [Display(Name = "Phone", Description = "Номер телефона")]
    Phone = 2,
    
    [Display(Name = "Telegram", Description = "Имя пользователя в Telegram")]
    Telegram = 3,
    
    [Display(Name = "Not supported", Description = "Неподдерживаемый тип контакта")]
    NotSupported = 9999
}