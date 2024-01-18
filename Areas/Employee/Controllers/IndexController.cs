using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using Microsoft.AspNetCore.Identity;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Areas.Employee.ViewModels;

namespace NexusConnectCRM.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee,Admin,HeadAdmin,Help Desk")]
    public class IndexController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexController(ApplicationDbContext context, 
                                  UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string id = _userManager.GetUserId(User);
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (id is null || user is null)
            {
                return NotFound();
            }

            EmployeeIndexViewModel viewModel = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View("Index", viewModel);
        }
    }
}
