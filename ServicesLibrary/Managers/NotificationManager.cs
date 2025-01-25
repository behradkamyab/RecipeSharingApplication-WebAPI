using DataAccessLayerLibrary.Interfaces;
using Microsoft.AspNetCore.SignalR;
using ModelsLibrary.Models;
using RecipeSharingWebApi.Hubs;
using ServicesLibrary.Exceptions.NotificationManagerExceptions;
using ServicesLibrary.Interfaces;

namespace ServicesLibrary.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationManager(INotificationRepository repo , IHubContext<NotificationHub> hub)
        {
            _notificationRepository = repo;
            _hubContext = hub;
        }
        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                var notifs = await _notificationRepository.GetAllNotificationsAsync(userId);
                if (notifs == null)
                {
                    throw new NotificationManagerException("notifications not found");
                }
                return notifs;
            }
            catch (Exception)
            {

                throw new NotificationManagerException("An Error Occurred!");
            }
        }



        public async Task MarkAsReadAsync(Guid notificationId)
        {
            try
            {
                var notif = await _notificationRepository.GetNotificationAsync(notificationId);
                if (notif  == null)
                {
                    throw new NotificationManagerException("notification not found");
                }
                notif.IsRead = true;  
                await _notificationRepository.UpdateNotification(notif);

            }
            catch (Exception)
            {

                throw new NotificationManagerException("An Error Occurred!") ;
            }
        }

        public async Task NotifyUserAsync(string userId, string message, string type)
        {
            try
            {
                var notif = new Notification
                {
                    Message = message,
                    Type = type,
                    ReceiverUserId = userId
                };
                await _notificationRepository.AddNotificationAsync(notif);
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification" , notif);
            }
            catch (Exception)
            {

                throw new NotificationManagerException("An Error Occurred!");
            }
        }
    }
}
