using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Admin.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class IndexController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();

            foreach (ApplicationUser user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                user.Roles = userRoles.FirstOrDefault();
            }

            if (User.IsInRole("Admin"))
            {
                foreach (var user in users.ToList())
                {
                    if (user.Roles == "Admin")
                    {
                        users.Remove(user);
                    }
                }
            }

            AdminIndexViewModel viewModel = new()
            {
                Users = users,
                TotalUsers = users.Count
            };

            return View("Index", viewModel);
        }

        public async Task<IActionResult> ViewProspects()
        {
            var users = await _context.Prospects.ToListAsync();

            if (users is null)
            {
                return NotFound();
            }

            AdminListProspectsViewModel viewModel = new()
            {
                Users = users
            };

            return View("ViewProspects", viewModel);
        }

        public async Task<IActionResult> ViewCustomers()
        {
            var users = await _context.Customers.ToListAsync();

            if (users is null)
            {
                return NotFound();
            }

            AdminListCustomersViewModel viewModel = new()
            {
                Users = users
            };

            return View("ViewCustomers", viewModel);
        }

        public async Task<IActionResult> ViewCompanies()
        {
            var companies = await _context.Companies.ToListAsync();

            if (companies is null)
            {
                return NotFound();
            }

            AdminListCompaniesViewModel viewModel = new()
            {
                Companies = companies
            };

            return View("ViewCompanies", viewModel);
        }
    }
}
