using Microsoft.AspNetCore.SignalR;

namespace NexusConnectCRM.Extensions.SignalR
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToUser(string receiverId, string senderName, string senderId, string message)
        {
            await Clients.User(receiverId).SendAsync("ReceiveMessageFromEmployee", senderName, senderId, message);
        }

        public async Task SendMessageToEmployee(string receiverId, string senderName, string senderId, string message)
        {
            await Clients.User(receiverId).SendAsync("ReceiveMessageFromUser", senderName, senderId, message);
        }

        public async Task DisplayMessageToSelf(string senderName, string senderId, string message)
        {
            await Clients.Caller.SendAsync("ReceiveMessageFromSelf", senderName, senderId, message);
        }
    }
}
