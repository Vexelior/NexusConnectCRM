using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Company;

namespace NexusConnectCRM.Controllers
{
    [Authorize(Roles = "Admin,Prospect,Customer,Employee")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<IActionResult> UpdateUserCompanyDetails(int? id)
        //{
        //    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

        //    if (id == null || _context.Companies == null || user == null)
        //    {
        //        return NotFound();
        //    }

        //    var userCompany = await _context.Companies.FirstOrDefaultAsync(x => x.Id == user.CompanyId);

        //    if (userCompany == null)
        //    {
        //        return NotFound();
        //    }

        //    return View("UpdateUserCompanyDetails", userCompany);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserCompanyDetails(int id, [Bind("Id,Name,Address,Country,City,State,Zip,Phone,Website,Email,Industry")] Company company)
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var companyExists = await _context.Companies.AnyAsync(x => x.Name == company.Name && x.Industry == company.Industry);
                if (companyExists)
                {
                    ModelState.AddModelError("", "A company with the same name and industry already exists.");
                    return View("UpdateUserCompanyDetails", company);
                }

                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new Exception($"Company {company.Name} could not be updated.");
                }
                return RedirectToAction("Index", "Prospect");
            }
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            return View("UpdateUserCompanyDetails", company);
        }
    }
}
