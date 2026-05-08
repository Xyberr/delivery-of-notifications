using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Notifications.API.Common;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var member = value.GetType()
            .GetMember(value.ToString())
            .First();

        return member
            .GetCustomAttribute<DisplayAttribute>()?
            .Name ?? value.ToString();
    }

    public static string? GetDisplayDescription(this Enum value)
    {
        var member = value.GetType()
            .GetMember(value.ToString())
            .FirstOrDefault();

        return member?
            .GetCustomAttribute<DisplayAttribute>()?
            .Description;
    }
}