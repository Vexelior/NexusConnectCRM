using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using Microsoft.AspNetCore.Identity;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Areas.Employee.ViewModels;
using NexusConnectCRM.Data.Models.Customer;

namespace NexusConnectCRM.Areas.Employee.Controllers
{
    [Authorize(Roles = "Employee, Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string id = _userManager.GetUserId(User);

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (id == null || user == null)
            {
                return NotFound();
            }

            EmployeeIndexViewModel viewModel = new()
            {
                EmployeeId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View("~/Areas/Employee/Views/Employee/Index.cshtml", viewModel);
        }

        public async Task<IActionResult> ViewProspects()
        {
            var users = await _context.Users.Where(x => x.Roles.Equals("Prospect")).ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            ListProspectsViewModel viewModel = new()
            {
                Users = users
            };

            return View("~/Areas/Employee/Views/Employee/ViewProspects.cshtml", viewModel);
        }

        public async Task<IActionResult> ViewCustomers()
        {
            var users = await _context.Users.Where(x => x.Roles.Equals("Customer")).ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            ListCustomersViewModel viewModel = new()
            {
                Users = users
            };

            return View("~/Areas/Employee/Views/Employee/ViewCustomers.cshtml", viewModel);
        }

        public async Task<IActionResult> ViewCompanies()
        {
            var companies = await _context.Companies.ToListAsync();

            if (companies == null)
            {
                return NotFound();
            }

            ListCompaniesViewModel viewModel = new()
            {
                Companies = companies
            };

            return View("~/Areas/Employee/Views/Employee/ViewCompanies.cshtml", viewModel);
        }
    }
}
