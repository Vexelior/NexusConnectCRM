using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Migrations;
using NexusConnectCRM.Data.Models.Company;

namespace NexusConnectCRM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,HeadAdmin")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EditCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company is null)
            {
                return NotFound();
            }

            return View("EditCompany", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCompany(CompanyInfo company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(EditCompany), new { id = company.Id });
                }

                return RedirectToAction("ViewCompanies", "Index", new { area = "Admin" });
            }

            return View("EditCompany", company);
        }

        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company is null)
            {
                return NotFound();
            }

            try
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(EditCompany), new { id = company.Id });
            }

            return RedirectToAction("ViewCompanies", "Index", new { area = "Admin" });
        }
    }
}
