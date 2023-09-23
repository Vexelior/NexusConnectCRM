﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Prospect.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Prospect;

namespace NexusConnectCRM.Areas.Prospect.Controllers
{
    [Area("Prospect")]
    [Authorize(Roles = "Admin,Employee,Prospect")]
    public class ProspectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProspectController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string user = User.Identity?.Name;

            if (!string.IsNullOrEmpty(user))
            {
                ProspectInfo verifiedUser = await _context.Prospects.FirstOrDefaultAsync(u => u.EmailAddress == user);

                if (verifiedUser == null)
                {
                    return NotFound();
                }

                CompanyInfo userCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == verifiedUser.CompanyId);

                if (userCompany == null)
                {
                    return await ProspectCompanyDetails(verifiedUser.UserId);
                }
                else if (verifiedUser.Address == null ||
                         verifiedUser.Country == null ||
                         verifiedUser.City == null ||
                         verifiedUser.State == null ||
                         verifiedUser.ZipCode == null ||
                         verifiedUser.PhoneNumber == null)
                {
                    return await CompleteUserDetails(Convert.ToString(verifiedUser.Id));
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
                    CompanyEmailAddress = userCompany.Email
                };

                return View("Index", viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> ProspectCompanyDetails(string id)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            ProspectCompanyDetailsViewModel viewModel = new(user);

            return View("EnterCompanyDetails", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CompanyDetails(ProspectCompanyDetailsViewModel viewModel)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == viewModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ProspectCompanyDetailsViewModel companyModel = new()
                {
                    UserId = viewModel.UserId,
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    Country = viewModel.Country,
                    City = viewModel.City,
                    State = viewModel.State,
                    ZipCode = viewModel.ZipCode,
                    PhoneNumber = viewModel.PhoneNumber,
                    Website = viewModel.Website,
                    Email = viewModel.Email,
                    Industry = viewModel.Industry
                };

                CompanyInfo company = new()
                {
                    Name = companyModel.Name,
                    Address = companyModel.Address,
                    Country = companyModel.Country,
                    City = companyModel.City,
                    State = companyModel.State,
                    Zip = companyModel.ZipCode,
                    Phone = companyModel.PhoneNumber,
                    Website = companyModel.Website,
                    Email = companyModel.Email,
                    Industry = companyModel.Industry
                };

                CompanyInfo potentialCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Name == company.Name &&
                                                                                             c.Industry == company.Industry);
                if (potentialCompany != null)
                {
                    user.CompanyId = potentialCompany.Id;
                    user.CompanyName = potentialCompany.Name;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Add(company);
                    await _context.SaveChangesAsync();

                    user.CompanyId = company.Id;
                    user.CompanyName = company.Name;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Prospect");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Company Details");
                return View("EnterCompanyDetails");
            }
        }

        public async Task<IActionResult> CompleteUserDetails(string id)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => Convert.ToString(u.Id) == id);

            if (user == null)
            {
                return NotFound();
            }

            ProspectUserDetailsViewModel viewModel = new(user);

            return View("EnterUserDetails", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ProspectDetails(ProspectUserDetailsViewModel viewModel)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == viewModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ProspectUserDetailsViewModel prospectModel = new()
                {
                    UserId = viewModel.UserId,
                    Address = viewModel.Address,
                    Country = viewModel.Country,
                    City = viewModel.City,
                    State = viewModel.State,
                    ZipCode = viewModel.ZipCode
                };

                user.Address = prospectModel.Address;
                user.Country = prospectModel.Country;
                user.City = prospectModel.City;
                user.State = prospectModel.State;
                user.ZipCode = prospectModel.ZipCode;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Prospect");
            }
            else
            {
                ModelState.AddModelError("", "Invalid User Details");
                return View("ProspectEnterUserDetails");
            }
        }
    }
}
