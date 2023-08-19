using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Prospect.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Migrations;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Prospect.Controllers
{
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
                ApplicationUser verifiedUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user);

                if (verifiedUser == null)
                {
                    return NotFound();
                }
                else
                {
                    ProspectIndexViewModel viewModel = new()
                    {
                        User = verifiedUser
                    };

                    if (verifiedUser.CompanyId == 0)
                    {
                        return await ProspectCompanyDetails(verifiedUser.Id);
                    }
                    else
                    {
                        return View("~/Areas/Prospect/Views/Prospect/Index.cshtml", viewModel);
                    }
                }
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> ProspectCompanyDetails(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ProspectCompanyDetailsViewModel viewModel = new(user);

            return View("~/Areas/Prospect/Views/Prospect/EnterCompanyDetails.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CompanyDetails(ProspectCompanyDetailsViewModel viewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == viewModel.UserId);

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

                Company company = new()
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

                var potentialCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Name == company.Name &&
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

                ProspectIndexViewModel indexViewModel = new()
                {
                    User = user
                };

                return View("~/Areas/Prospect/Views/Prospect/Index.cshtml", indexViewModel);
            }
            else
            {
                ModelState.AddModelError("", "Invalid Company Details");
                return View("~/Areas/Prospect/Views/Prospect/EnterCompanyDetails.cshtml");
            }
        }

        public async Task<IActionResult> CompleteUserDetails(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ProspectUserDetailsViewModel viewModel = new(user);

            return View("~/Areas/Prospect/Views/Prospect/EnterUserDetails.cshtml", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ProspectDetails(ProspectUserDetailsViewModel viewModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == viewModel.UserId);

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
                    ZipCode = viewModel.ZipCode,
                };

                user.Address = prospectModel.Address;
                user.Country = prospectModel.Country;
                user.City = prospectModel.City;
                user.State = prospectModel.State;
                user.ZipCode = prospectModel.ZipCode;

                _context.Update(user);
                await _context.SaveChangesAsync();

                ProspectIndexViewModel indexViewModel = new()
                {
                    User = user
                };

                return View("~/Areas/Prospect/Views/Prospect/Index.cshtml", indexViewModel);
            }
            else
            {
                ModelState.AddModelError("", "Invalid User Details");
                return View("~/Areas/Prospect/Views/Prospect/ProspectEnterUserDetails.cshtml");
            }
        }
    }
}
