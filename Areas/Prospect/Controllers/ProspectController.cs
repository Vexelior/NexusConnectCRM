using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Prospect.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Help;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;
using System.Drawing;

namespace NexusConnectCRM.Areas.Prospect.Controllers
{
    [Area("Prospect")]
    [Authorize(Roles = "Admin,Employee,Prospect")]
    public class ProspectController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProspectController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string user = _userManager.GetUserId(User);

            if (!string.IsNullOrEmpty(user))
            {
                ProspectInfo verifiedUser = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == user);

                if (verifiedUser == null)
                {
                    return NotFound();
                }

                CompanyInfo userCompany = await _context.Companies.FirstOrDefaultAsync(c => c.Id == verifiedUser.CompanyId);

                if (userCompany == null)
                {
                    return await ProspectCompanyDetails(verifiedUser.UserId);
                }
                else if (verifiedUser.Address is null ||
                         verifiedUser.Country is null ||
                         verifiedUser.City is null ||
                         verifiedUser.State is null ||
                         verifiedUser.ZipCode is null ||
                         verifiedUser.PhoneNumber is null)
                {
                    return await CompleteUserDetails(verifiedUser.UserId);
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

            return View("EnterCompanyDetails");
        }

        [HttpPost]
        public async Task<IActionResult> CompanyDetails([Bind("Id,Name,Address,Country,City,State,Zip,Phone,Website,Email,Industry,NeedsContact")] CompanyInfo companyInfo)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == _userManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _context.Companies.AddAsync(companyInfo);
                await _context.SaveChangesAsync();

                user.CompanyId = companyInfo.Id;
                user.CompanyName = companyInfo.Name;
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Prospect");
            }
            else
            {
                return View("EnterCompanyDetails");
            }
        }

        public async Task<IActionResult> CompleteUserDetails(string id)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View("EnterUserDetails");
        }


        [HttpPost]
        public async Task<IActionResult> ProspectDetails([Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,Address,City,State,ZipCode,Country,CompanyName,PhoneNumber,CompanyId,UserId,IsActive,IsContacted,IsHelped,NeedsHelp,CreatedDate,ModifiedDate")] ProspectInfo prospectInfo)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == _userManager.GetUserId(User));

            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.Address = prospectInfo.Address;
                user.City = prospectInfo.City;
                user.State = prospectInfo.State;
                user.ZipCode = prospectInfo.ZipCode;
                user.Country = prospectInfo.Country;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Prospect");
            }

            return View("EnterUserDetails");
        }
    }
}
