﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using Microsoft.AspNetCore.Identity;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Areas.Employee.ViewModels;

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
                                                      p.ZipCode != null);

            int prospectsNeededContactCount = prospectsNeededContact.Count();

            var customersNeededContact = _context.Customers.Where(c => c.NeedsContact == true);
            int customersNeededContactCount = customersNeededContact.Count();

            var companiesNeededContact = _context.Companies.Where(c => c.NeedsContact == true);
            int companiesNeededContactCount = companiesNeededContact.Count();

            var totalNeededContactCount = prospectsNeededContactCount + customersNeededContactCount + companiesNeededContactCount;

            var prospectsNotNeededContact = _context.Prospects.Where(p => p.IsContacted == true &&
                                                                 p.Address != null &&
                                                                 p.City != null &&
                                                                 p.State != null &&
                                                                 p.Country != null &&
                                                                 p.ZipCode != null);
            var prospectsNotNeededContactCount = prospectsNotNeededContact.Count();

            var customersNotNeededContact = _context.Customers.Where(c => c.NeedsContact == false);
            var customersNotNeededContactCount = customersNotNeededContact.Count();

            var companiesNotNeededContact = _context.Companies.Where(c => c.NeedsContact == false);
            var companiesNotNeededContactCount = companiesNotNeededContact.Count();

            var totalNotNeededContactCount = prospectsNotNeededContactCount + customersNotNeededContactCount + companiesNotNeededContactCount;


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
