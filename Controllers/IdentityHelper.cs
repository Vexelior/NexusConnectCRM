using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Controllers
{
    public class IdentityHelper(UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet]
        public async Task<string> GetName()
        {
            var id = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User));
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user is null || id is null)
            {
                return "Invalid User";
            }

            System.Diagnostics.Debug.WriteLine(user.FirstName + " " + user.LastName);

            return user.FirstName + " " + user.LastName;
        }
    }
}
