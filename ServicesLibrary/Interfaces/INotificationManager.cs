using ModelsLibrary.Models;

namespace ServicesLibrary.Interfaces
{
    // for simplicity, notification manager response has been avoided!
    public interface INotificationManager
    {
        Task NotifyUserAsync(string userId, string message, string type);
        Task MarkAsReadAsync(Guid notificationId);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
    }
}
