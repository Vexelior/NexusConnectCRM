using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Customer.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Customer,Admin,Employee")]
    public class IndexController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IActionResult> Index()
        {
            if (_userManager.GetUserId(User) is null)
            {
                return NotFound();
            }

            var verifiedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == _userManager.GetUserId(User));
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == customer.CompanyId);
            var userRoles = await _userManager.GetRolesAsync(verifiedUser);

            CustomerIndexViewModel viewModel = new()
            {
                Customer = customer,
                Company = company,
                Role = userRoles[0]
            };

            return View("Index", viewModel);
        }
    }
}
