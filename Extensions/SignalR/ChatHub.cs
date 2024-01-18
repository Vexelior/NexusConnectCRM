using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;

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
            bool isHelpDeskAvailable = await CheckIfHelpDeskIsAvailable();

            if (isHelpDeskAvailable == true)
            {
                await Clients.User(receiverId).SendAsync("ReceiveMessageFromUser", senderName, senderId, message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessageFromEmployee", "System", "Help Desk", "Sorry, the help desk is currently unavailable. Please try again later.");
            }
        }

        public async Task DisplayMessageToSelf(string senderName, string senderId, string message)
        {
            senderName = "You";
            await Clients.Caller.SendAsync("ReceiveMessageFromSelf", senderName, senderId, message);
        }

        public async Task<bool> CheckIfHelpDeskIsAvailable()
        {
            UserManager<ApplicationUser> _userManager = new(new UserStore<ApplicationUser>(new ApplicationDbContext()), null, null, null, null, null, null, null, null);
            var helpDesk = await _userManager.FindByNameAsync("helpdesk@mail.com");

            if (helpDesk == null)
            {
                return false;
            }
            else if (helpDesk.IsOnline == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
