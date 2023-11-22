using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Prospect.Data;
using NexusConnectCRM.Areas.Prospect.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;

namespace NexusConnectCRM.Areas.Prospect.Controllers
{
    [Area("Prospect")]
    [Authorize(Roles = "Admin,Employee,Prospect")]
    public class DetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsController(ApplicationDbContext context, 
                                 UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult CompanyDetails()
        {
            var countryDetails = Countries.GetAll();
            var stateDetails = States.GetAll();

            ProspectCompanyDetailsViewModel model = new()
            {
                ListOfCountries = new List<SelectListItem>(),
                ListOfStates = new List<SelectListItem>()
            };

            foreach (var country in countryDetails)
            {
                model.ListOfCountries.Add(new SelectListItem { Value = country.Id, Text = country.Name });
            }

            foreach (var state in stateDetails)
            {
                model.ListOfStates.Add(new SelectListItem { Value = state.Id, Text = state.Name });
            }

            return View("EnterCompanyDetails", model);
        }

        [HttpPost]
        public async Task<IActionResult> CompanyDetails([Bind("Id,Name,Address,Country,City,State,Zip,Phone,Website,Email,Industry,NeedsContact")] CompanyInfo companyInfo)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == _userManager.GetUserId(User));

            if (user is null)
            {
                return NotFound();
            }

            var countryDetails = Countries.GetAll();
            var stateDetails = States.GetAll();

            foreach (var country in countryDetails)
            {
                if (country.Id == companyInfo.Country)
                {
                    companyInfo.Country = country.Name;
                }
            }

            foreach (var state in stateDetails)
            {
                if (state.Id == companyInfo.State)
                {
                    companyInfo.State = state.Name;
                }
            }

            if (ModelState.IsValid)
            {
                await _context.Companies.AddAsync(companyInfo);
                await _context.SaveChangesAsync();

                user.CompanyId = companyInfo.Id;
                user.CompanyName = companyInfo.Name;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Index", null);
            }
            else
            {
                return View("EnterCompanyDetails");
            }
        }

        public IActionResult UserDetails()
        {
            var countryDetails = Countries.GetAll();
            var stateDetails = States.GetAll();

            ProspectUserDetailsViewModel model = new()
            {
                ListOfCountries = new List<SelectListItem>(),
                ListOfStates = new List<SelectListItem>()
            };

            foreach (var country in countryDetails)
            {
                model.ListOfCountries.Add(new SelectListItem { Value = country.Id, Text = country.Name });
            }

            foreach (var state in stateDetails)
            {
                model.ListOfStates.Add(new SelectListItem { Value = state.Id, Text = state.Name });
            }

            return View("EnterUserDetails", model);
        }

        [HttpPost]
        public async Task<IActionResult> UserDetails([Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,Address,City,State,ZipCode,Country,CompanyName,PhoneNumber,CompanyId,UserId,IsActive,IsContacted,IsHelped,NeedsHelp,CreatedDate,ModifiedDate")] ProspectInfo prospectInfo)
        {
            var user = await _context.Prospects.FirstOrDefaultAsync(u => u.UserId == _userManager.GetUserId(User));

            if (user is null)
            {
                return NotFound();
            }

            var countryDetails = Countries.GetAll();
            var stateDetails = States.GetAll();

            foreach (var country in countryDetails)
            {
                if (country.Id == prospectInfo.Country)
                {
                    prospectInfo.Country = country.Name;
                }
            }

            foreach (var state in stateDetails)
            {
                if (state.Id == prospectInfo.State)
                {
                    prospectInfo.State = state.Name;
                }
            }

            if (ModelState.IsValid)
            {
                user.Address = prospectInfo.Address;
                user.City = prospectInfo.City;
                user.State = prospectInfo.State;
                user.ZipCode = prospectInfo.ZipCode;
                user.Country = prospectInfo.Country;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Index", null);
            }

            return View("EnterUserDetails");
        }
    }
}
