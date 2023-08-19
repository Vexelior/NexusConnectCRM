using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Admin.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ApplicationUser> users = await _context.Users.ToListAsync();

            foreach (ApplicationUser user in users)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                user.Roles = userRoles.FirstOrDefault();
            }

            AdminIndexViewModel viewModel = new()
            {
                Users = users
            };

            return View("~/Areas/Admin/Views/Admin/Index.cshtml", viewModel);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            List<SelectListItem> selectListItems = new();

            foreach (var role in _context.Roles)
            {
                bool isSelected = false;
                if (userRoles != null && userRoles.Contains(role.Name))
                {
                    isSelected = true;
                }
                selectListItems.Add(new SelectListItem(role.Name.Humanize(LetterCasing.Title), role.Name, isSelected));
            }

            AdminEditViewModel viewModel = new(user)
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = DateTime.Parse(user.DateOfBirth.ToString()).ToString("MM/dd/yyyy"),
                Address = user.Address,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                Country = user.Country,
                CompanyName = user.CompanyName,
                Roles = selectListItems
            };

            return View("~/Areas/Admin/Views/Admin/EditUser.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AdminEditViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            viewModel.Roles = new List<SelectListItem> { new SelectListItem { Value = viewModel.SelectedRole, Text = viewModel.SelectedRole } };

            if (ModelState.IsValid)
            {
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Email = viewModel.Email;
                user.DateOfBirth = Convert.ToDateTime(viewModel.DateOfBirth);
                user.Address = viewModel.Address;
                user.City = viewModel.City;
                user.State = viewModel.State;
                user.ZipCode = viewModel.ZipCode;
                user.Country = viewModel.Country;
                user.CompanyName = viewModel.CompanyName;

                if (string.IsNullOrEmpty(viewModel.SelectedRole))
                {
                    ModelState.AddModelError("", "Please select a role");
                    return View("~/Areas/Admin/Views/Admin/EditUser.cshtml", viewModel);
                }

                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
                var secondResult = await _userManager.AddToRoleAsync(user, viewModel.SelectedRole);

                if (result.Succeeded && secondResult.Succeeded)
                {
                    user.Roles = viewModel.SelectedRole;
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong...");
                    return View("~/Areas/Admin/Views/Admin/EditUser.cshtml", viewModel);
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong...");
                return View("~/Areas/Admin/Views/Admin/EditUser.cshtml", viewModel);
            }

            IList<string> newUserRoles = await _userManager.GetRolesAsync(user);
            List<SelectListItem> selectListItems = new();

            foreach (var role in _context.Roles)
            {
                selectListItems.Add(new SelectListItem(role.Name.Humanize(LetterCasing.Title), role.Name, newUserRoles.Contains(role.Name)));
            }

            IEnumerable<ApplicationUser> users = await _context.Users.ToListAsync();

            AdminIndexViewModel newModel = new()
            {
                Users = users
            };

            // Add query string to index view
            return View("~/Areas/Admin/Views/Admin/Index.cshtml", newModel);
        }
    }
}
