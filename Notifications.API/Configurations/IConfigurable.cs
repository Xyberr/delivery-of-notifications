namespace Notifications.API.Entities;

public interface IConfigurable
{
    static abstract string SectionName { get; }
}