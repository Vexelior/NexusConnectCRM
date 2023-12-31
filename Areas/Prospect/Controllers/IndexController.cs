﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Prospect.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;

namespace NexusConnectCRM.Areas.Prospect.Controllers
{
    [Area("Prospect")]
    [Authorize(Roles = "Admin,Employee,Prospect")]
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
            string user = _userManager.GetUserId(User);

            ProspectInfo verifiedUser = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == user);

            if (user is null || verifiedUser is null)
            {
                return NotFound();
            }

            CompanyInfo userCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == verifiedUser.CompanyId);

            if (userCompany is null)
            {
                return RedirectToAction("CompanyDetails", "Details", null);
            }
            else if (verifiedUser.Address is null ||
                     verifiedUser.Country is null ||
                     verifiedUser.City is null ||
                     verifiedUser.State is null ||
                     verifiedUser.ZipCode is null ||
                     verifiedUser.PhoneNumber is null)
            {
                return RedirectToAction("UserDetails", "Details", null);
            }

            var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == verifiedUser.UserId);
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == userRole.RoleId);

            ProspectIndexViewModel viewModel = new()
            {
                // User Details. \\
                FirstName = verifiedUser.FirstName,
                LastName = verifiedUser.LastName,
                Address = verifiedUser.Address,
                City = verifiedUser.City,
                State = verifiedUser.State,
                ZipCode = verifiedUser.ZipCode,
                Country = verifiedUser.Country,
                PhoneNumber = verifiedUser.PhoneNumber,
                EmailAddress = verifiedUser.EmailAddress,
                UserRole = role.Name,
                // Company Details. \\
                CompanyName = userCompany.Name,
                CompanyAddress = userCompany.Address,
                CompanyCity = userCompany.City,
                CompanyState = userCompany.State,
                CompanyZipCode = userCompany.Zip,
                CompanyPhoneNumber = userCompany.Phone,
                CompanyEmailAddress = userCompany.Email,
            };

            return View("Index", viewModel);
        }
    }
}
