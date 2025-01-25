using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace RecipeSharingWebApi.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Get the user ID from the claims
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                // Add the user to a group (optional)
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }
        public async Task SendNotificationToUser(string userId, string message)
        {
            // Ensure the user is authenticated
            if (Context.User?.Identity?.IsAuthenticated != true)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Send the notification to the specific user
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

    }
}
