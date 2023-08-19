using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Customer.ViewModels;
using NexusConnectCRM.Data;

namespace NexusConnectCRM.Areas.Customer.Controllers
{
    [Authorize(Roles = "Customer,Admin,Employee")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string user = User.Identity?.Name;

            if (string.IsNullOrEmpty(user))
            {
                return NotFound();

            }

            CustomerIndexViewModel viewModel = new()
            {
                User = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user)
            };

            return View("~/Areas/Customer/Views/Customer/Index.cshtml", viewModel);
        }
    }
}
