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
    [Authorize(Roles = "Employee, Admin")]
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

            var prospectsNeededContact = _context.Prospects.Where(p => p.IsContacted == false &&
                                                                               p.Address != null &&
                                                                               p.City != null &&
                                                                               p.State != null &&
                                                                               p.Country != null &&
                                                                               p.ZipCode != null).Count();

            var customersNeededContact = _context.Customers.Where(c => c.NeedsContact == true).Count();
            var companiesNeededContact = _context.Companies.Where(c => c.NeedsContact == true).Count();


            var prospectsNotNeededContact = _context.Prospects.Where(p => p.IsContacted == true &&
                                                                                  p.Address != null &&
                                                                                  p.City != null &&
                                                                                  p.State != null &&
                                                                                  p.Country != null &&
                                                                                  p.ZipCode != null).Count();

            var customersNotNeededContact = _context.Customers.Where(c => c.NeedsContact == false).Count();
            var companiesNotNeededContact = _context.Companies.Where(c => c.NeedsContact == false).Count();

            var totalNeededContactCount = prospectsNeededContact + customersNeededContact + companiesNeededContact;
            var totalNotNeededContactCount = prospectsNotNeededContact + customersNotNeededContact + companiesNotNeededContact;


            EmployeeIndexViewModel viewModel = new()
            {
                EmployeeId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TotalTasks = totalNeededContactCount + totalNotNeededContactCount,
                UncompletedTasks = totalNeededContactCount,
                CompletedTasks = totalNotNeededContactCount
            };

            return View("Index", viewModel);
        }
    }
}
