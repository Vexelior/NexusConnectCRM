using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Controllers
{
    public class IdentityHelper(UserManager<ApplicationUser> userManager,
                                ApplicationDbContext context) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        [HttpGet]
        public async Task<string> GetName()
        {
            var id = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user is null || id is null)
            {
                return "Invalid User";
            }

            return user.FirstName + " " + user.LastName;
        }

        [HttpGet]
        public async Task<int> GetTotalEmployeeRespondedTickets(string userId)
        {
            return await _context.Help.Where(h => h.Author == userId && h.EmployeeWasRecentResponse).CountAsync();
        }
    }
}
