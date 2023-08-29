using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using Microsoft.AspNetCore.Identity;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Areas.Employee.ViewModels;
using NexusConnectCRM.Data.Models.Help;

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

            return View("~/Areas/Employee/Views/Employee/Index.cshtml", viewModel);
        }

        public async Task<IActionResult> ViewProspects()
        {
            var users = await _context.Prospects.ToListAsync();

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
            var users = await _context.Customers.ToListAsync();

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

        public async Task<IActionResult> Help()
        {
            var helpList = await _context.Help.ToListAsync();

            if (helpList == null)
            {
                return NotFound();
            }

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("~/Areas/Employee/Views/Employee/Help.cshtml", viewModel);
        }

        public async Task<IActionResult> HelpApprove(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            help.IsCompleted = false;
            help.IsApproved = true;
            help.IsPending = false;
            help.IsRejected = false;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction("Help");
        }

        public async Task<IActionResult> HelpReject(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            help.IsCompleted = true;
            help.IsApproved = false;
            help.IsPending = false;
            help.IsRejected = true;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction("Help");
        }

        public async Task<IActionResult> NotApprovedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsApproved == false).ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("~/Areas/Employee/Views/Employee/NotApprovedHelp.cshtml", viewModel);
        }

        public async Task<IActionResult> NotCompletedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsCompleted == false).ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("~/Areas/Employee/Views/Employee/NotCompletedHelp.cshtml", viewModel);
        }

        public async Task<IActionResult> RejectedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsRejected == true).ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("~/Areas/Employee/Views/Employee/NotCompletedHelp.cshtml", viewModel);
        }

        public async Task<IActionResult> CompletedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsCompleted == true).ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("~/Areas/Employee/Views/Employee/NotCompletedHelp.cshtml", viewModel);
        }

        public async Task<IActionResult> HelpEdit(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            List<HelpResponseInfo> feedback = await _context.HelpFeedback.Where(h => h.ResponseId == help.ResponseId).ToListAsync();

            HelpEditViewModel viewModel = new()
            {
                Id = id,
                Help = help,
                HelpResponses = feedback
            };

            return View("~/Areas/Employee/Views/Employee/HelpEdit.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitHelpResponse(int id, HelpEditViewModel viewModel)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);
            
            if (help == null)
            {
                return NotFound();
            }

            if (viewModel.Response == null)
            {
                ModelState.AddModelError("Response", "Please enter a response.");
                return RedirectToAction("HelpEdit", new { id = id });
            }

            string name = _context.Users.FirstOrDefault(u => u.Id == help.Author).FirstName + " " + _context.Users.FirstOrDefault(u => u.Id == help.Author).LastName;

            DateTime date = DateTime.Now;
            string dateString = date.ToString("MM/dd/yyyy h:mmtt");

            string message = $"[{dateString}] {name}: \r {viewModel.Response}";

            HelpResponseInfo feedback = new()
            {
                Response = message,
                Author = _userManager.GetUserId(User),
                IsEmployee = true,
                CreatedDate = help.CreatedDate,
                ModifiedDate = DateTime.Now,
                Image = null,
                ResponseId = help.ResponseId
            };

            help.IsPending = false;
            help.IsCompleted = true;
            help.IsApproved = true;
            help.IsRejected = false;

            _context.Add(feedback);
            await _context.SaveChangesAsync();

            HelpEditViewModel vm = new()
            {
                Id = id,
                Help = help,
                HelpResponses = await _context.HelpFeedback.Where(h => h.ResponseId == help.ResponseId).ToListAsync(),
                Response = null
            };

            return View("~/Areas/Employee/Views/Employee/HelpEdit.cshtml", vm);
        }

        public async Task<IActionResult> ProspectMarkContacted(int id)
        {
            var prospect = await _context.Prospects.FirstOrDefaultAsync(m => m.Id == id);

            if (prospect == null)
            {
                return NotFound();
            }

            prospect.IsContacted = true;

            _context.Update(prospect);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewProspects", "Employee");
        }

        public async Task<IActionResult> ProspectHelped(int id)
        {
            var prospect = await _context.Prospects.FirstOrDefaultAsync(m => m.Id == id);

            if (prospect == null)
            {
                return NotFound();
            }

            prospect.IsHelped = false;

            _context.Update(prospect);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCustomers", "Employee");
        }

        public async Task<IActionResult> CustomerHelped(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            customer.NeedsContact = false;

            _context.Update(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCustomers", "Employee");
        }

        public async Task<IActionResult> CompanyHelped(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(m => m.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            company.NeedsContact = false;

            _context.Update(company);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewCompanies", "Employee");
        }
    }
}
