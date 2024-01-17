using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Admin.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;

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
            var users = await _context.Users.Where(x => !x.Roles.Contains("Employee") &&
                                                                !x.Roles.Contains("Admin"))
                                            .OrderBy(x => x.FirstName)
                                            .ToListAsync();

            AdminIndexViewModel viewModel = new()
            {
                Users = users,
                TotalUsers = users.Count
            };

            return View("Index", viewModel);
        }

        public async Task<IActionResult> ViewUsers(string searchQuery, int page = 1, int pageSize = 10)
        {
            List<ApplicationUser> users = [];

            var IsAdmin = User.IsInRole("Admin");
            var IsHeadAdmin = User.IsInRole("HeadAdmin");

            searchQuery ??= "";

            IQueryable<ApplicationUser> query = _context.Users;

            if (IsAdmin)
            {
                query = query.Where(x => !x.Roles.Contains("Employee") &&
                                         !x.Roles.Contains("Admin") &&
                                         !x.Roles.Contains("Help Desk") &&
                                         (x.FirstName.Contains(searchQuery) || 
                                         x.LastName.Contains(searchQuery)) ||
                                         x.Email.Contains(searchQuery));
            }
            else if (IsHeadAdmin)
            {
                query = query.Where(x => x.FirstName.Contains(searchQuery) ||
                                                  x.LastName.Contains(searchQuery) ||
                                                  x.Email.Contains(searchQuery));
            }

            int totalUsers = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            users = await query.OrderBy(x => x.Roles)
                               .ThenBy(x => x.FirstName)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

            if (users is null)
            {
                return NotFound();
            }

            AdminListUsersViewModel viewModel = new()
            {
                Users = users,
                SearchQuery = searchQuery,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalUsers = totalUsers
            };

            return View("ViewUsers", viewModel);
        }

        public async Task<IActionResult> ViewProspects(string searchQuery, int page = 1, int pageSize = 10)
        {
            List<ProspectInfo> prospects = [];

            IQueryable<ProspectInfo> query = _context.Prospects;

            searchQuery ??= "";

            query = query.Where(x => x.FirstName.Contains(searchQuery) ||
                                              x.LastName.Contains(searchQuery) ||
                                              x.EmailAddress.Contains(searchQuery) ||
                                              x.PhoneNumber.Contains(searchQuery));

            int totalProspects = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalProspects / pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            prospects = await query.OrderBy(x => x.FirstName)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            if (prospects is null)
            {
                return NotFound();
            }

            AdminListProspectsViewModel viewModel = new()
            {
                Prospects = prospects,
                SearchQuery = searchQuery,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalProspects = totalProspects
            };

            return View("ViewProspects", viewModel);
        }

        public async Task<IActionResult> ViewCustomers(string searchQuery, int page = 1, int pageSize = 10)
        {
            List<CustomerInfo> customers = [];

            searchQuery ??= "";

            IQueryable<CustomerInfo> query = _context.Customers;

            query = query.Where(x => x.FirstName.Contains(searchQuery) ||
                                              x.LastName.Contains(searchQuery) ||
                                              x.EmailAddress.Contains(searchQuery) ||
                                              x.PhoneNumber.Contains(searchQuery));

            int totalCustomers = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCustomers / pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            customers = await query.OrderBy(x => x.FirstName)
                                   .Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            if (customers is null)
            {
                return NotFound();
            }

            AdminListCustomersViewModel viewModel = new()
            {
                Customers = customers,
                SearchQuery = searchQuery,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCustomers = totalCustomers
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
