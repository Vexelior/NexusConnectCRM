﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using Microsoft.AspNetCore.Identity;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Areas.Employee.ViewModels;
using NexusConnectCRM.Data.Models.Help;

namespace NexusConnectCRM.Areas.Employee.Controllers
{
    [Area("Employee")]
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

            return View("Index", viewModel);
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

            return View("ViewProspects", viewModel);
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

            return View("ViewCustomers", viewModel);
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

            return View("ViewCompanies", viewModel);
        }

        public async Task<IActionResult> Help()
        {
            var helpList = await _context.Help.OrderByDescending(h => h.CreatedDate).ToListAsync();

            if (helpList == null)
            {
                return NotFound();
            }

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("Help", viewModel);
        }

        public async Task<IActionResult> HelpApprove(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            help.IsApproved = true;
            help.IsPending = false;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> HelpReject(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            help.IsApproved = false;
            help.IsRejected = true;
            help.IsClosed = false;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> HelpClose(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            help.IsCompleted = true;
            help.IsClosed = true;
            help.IsRejected = false;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> HelpCompleted(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id) ?? throw new Exception("Help ticket not found");

            help.IsCompleted = true;
            help.IsClosed = false;
            help.IsRejected = false;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> NewHelp()
        {
            var helpList = await _context.Help.Where(h => h.CustomerWasRecentResponse)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("NewHelp", viewModel);
        }

        public async Task<IActionResult> ClosedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsClosed)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("ClosedHelp", viewModel);
        }

        public async Task<IActionResult> PendingHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsPending)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("PendingHelp", viewModel);
        }

        public async Task<IActionResult> RejectedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsRejected)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("RejectedHelp", viewModel);
        }

        public async Task<IActionResult> CompletedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsCompleted)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("CompletedHelp", viewModel);
        }

        public async Task<IActionResult> HelpEdit(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            List<HelpResponseInfo> feedback = await _context.HelpFeedback.Where(h => h.ResponseId == help.Id).ToListAsync();

            HelpEditViewModel viewModel = new()
            {
                Id = id,
                Help = help,
                HelpResponses = feedback
            };

            await SetEmployeeViewed(id);

            return View("HelpEdit", viewModel);
        }

        public async Task SetEmployeeViewed(int id)
        {
            HelpInfo help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return;
            }
            try
            {
                help.EmployeeViewed = true;
                _context.Update(help);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelpExists(help.Id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }
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

            string name = _context.Users.Where(u => u.Id == _userManager.GetUserId(User)).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault();

            DateTime date = DateTime.Now;
            string dateString = date.ToString("MM/dd/yyyy h:mmtt");

            string message = $"[{dateString}] {name}: \n {viewModel.Response}";

            HelpResponseInfo feedback = new()
            {
                Response = message,
                Author = _userManager.GetUserId(User),
                IsEmployee = true,
                CreatedDate = help.CreatedDate,
                ModifiedDate = DateTime.Now,
                Image = null,
                ResponseId = help.Id
            };

            help.ModifiedDate = feedback.ModifiedDate;
            help.CustomerWasRecentResponse = false;
            help.EmployeeWasRecentResponse = true;

            _context.Add(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });

        }

        public async Task<IActionResult> ProspectContacted(int id)
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

        private bool HelpExists(int id)
        {
            return _context.Help.Any(e => e.Id == id);
        }
    }
}
