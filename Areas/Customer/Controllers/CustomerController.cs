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
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var verifiedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserId == _userManager.GetUserId(User));
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == customer.CompanyId);

            if (_userManager.GetUserId(User) is null)
            {
                return NotFound();
            }

            CustomerIndexViewModel viewModel = new()
            {
                User = verifiedUser,
                CustomerInfo = customer,
                Company = company
            };

            return View("Index", viewModel);
        }
    }
}
