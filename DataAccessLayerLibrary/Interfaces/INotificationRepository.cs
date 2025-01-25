

using ModelsLibrary.Models;

namespace DataAccessLayerLibrary.Interfaces
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task RemoveNotificationAsync(Guid notificationId);
        Task UpdateNotification(Notification notification);
        Task<Notification?> GetNotificationAsync(Guid notificationId);
        Task<IEnumerable<Notification>?> GetAllNotificationsAsync(string userId);
    }
}
