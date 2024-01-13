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
    [Authorize(Roles = "Admin,HeadAdmin")]
    public class IndexController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = [];

            var IsAdmin = User.IsInRole("Admin");
            var IsHeadAdmin = User.IsInRole("HeadAdmin");

            if (IsAdmin)
            {
                users = await _context.Users.Where(x => !x.Roles.Contains("Employee") &&
                                                     !x.Roles.Contains("Admin"))
                                                    .OrderBy(x => x.FirstName)
                                                    .ToListAsync();
            }
            else if (IsHeadAdmin)
            {
                users = await _context.Users.ToListAsync();
            }

            AdminIndexViewModel viewModel = new()
            {
                Users = users,
                TotalUsers = users.Count
            };

            return View("Index", viewModel);
        }

        public async Task<IActionResult> ViewUsers()
        {
            List<ApplicationUser> users = [];

            var IsAdmin = User.IsInRole("Admin");
            var IsHeadAdmin = User.IsInRole("HeadAdmin");

            if (IsAdmin)
            {
                users = await _context.Users.Where(x => !x.Roles.Contains("Employee") &&
                                                                 !x.Roles.Contains("Admin") &&
                                                                 !x.Roles.Contains("Help Desk"))
                                            .OrderBy(x => x.Roles)
                                            .ThenBy(x => x.FirstName)
                                            .ToListAsync();
            }
            else if (IsHeadAdmin)
            {
                // Sort by both Roles and FirstName
                users = await _context.Users.OrderBy(x => x.Roles)
                                                    .ThenBy(x => x.FirstName)
                                                    .ToListAsync();
            }

            if (users is null)
            {
                return NotFound();
            }

            AdminListUsersViewModel viewModel = new()
            {
                Users = users
            };

            return View("ViewUsers", viewModel);
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
