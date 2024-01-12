using Microsoft.AspNetCore.SignalR;

namespace NexusConnectCRM.Extensions.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task Notify(string sender, string receiver, string message)
        {
            await Clients.User(receiver).SendAsync("ReceiveNotification", sender, message);
        }
    }
}
