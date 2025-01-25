
using DataAccessLayerLibrary.DataPersistence;
using DataAccessLayerLibrary.Exceptions;
using DataAccessLayerLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;

namespace DataAccessLayerLibrary.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            try
            {
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }
        public async Task RemoveNotificationAsync(Guid notificationId)
        {
            try
            {
                await _context.Notifications.Where(n => n.Id == notificationId).ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task UpdateNotification(Notification notification)
        {
            try
            {
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }
        
        public async Task<IEnumerable<Notification>?> GetAllNotificationsAsync(string userId)
        {
            try
            {
               return await _context.Notifications.Where(n => n.ReceiverUserId == userId).OrderByDescending(n => n.CreatedAt).ToListAsync();
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

        public async Task<Notification?> GetNotificationAsync(Guid notificationId)
        {
            try
            {
                return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId);
            }
            catch (Exception ex)
            {

                throw new DataAccessException("An error occurred while accessing the Database!", ex);

            }
        }

       
    }
}
