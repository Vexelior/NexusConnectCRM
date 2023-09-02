using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Admin.ViewModels;
using NexusConnectCRM.Areas.Employee.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Employee;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;

namespace NexusConnectCRM.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            List<ApplicationUser> users = await _context.Users.ToListAsync();

            foreach (ApplicationUser user in users)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                user.Roles = userRoles.FirstOrDefault();
            }

            if (User.IsInRole("Admin"))
            {
                foreach (var user in users.ToList())
                {
                    if (user.Roles == "Admin")
                    {
                        users.Remove(user);
                    }
                }
            }

            AdminIndexViewModel viewModel = new()
            {
                Users = users
            };

            return View("Index", viewModel);
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

            if (user.Roles == "Prospect")
            {
                var potentialProspect = _context.Prospects.Where(p => p.UserId == user.Id).FirstOrDefault();

                if (potentialProspect != null)
                {
                    AdminEditViewModel viewModel = new()
                    {
                        FirstName = potentialProspect.FirstName,
                        LastName = potentialProspect.LastName,
                        EmailAddress = potentialProspect.EmailAddress,
                        DateOfBirth = potentialProspect.DateOfBirth.ToString(),
                        Address = potentialProspect.Address,
                        City = potentialProspect.City,
                        State = potentialProspect.State,
                        ZipCode = potentialProspect.ZipCode,
                        Country = potentialProspect.Country,
                        CompanyName = potentialProspect.CompanyName,
                        PhoneNumber = potentialProspect.PhoneNumber,
                        UserId = potentialProspect.UserId,
                        Roles = selectListItems
                    };

                    return View("EditUser", viewModel);
                }
            }
            else if (user.Roles == "Customer")
            {
                var potentialCustomer = _context.Customers.Where(c => c.UserId == user.Id).FirstOrDefault();

                if (potentialCustomer != null)
                {
                    AdminEditViewModel viewModel = new()
                    {

                        FirstName = potentialCustomer.FirstName,
                        LastName = potentialCustomer.LastName,
                        EmailAddress = potentialCustomer.EmailAddress,
                        DateOfBirth = potentialCustomer.DateOfBirth.ToString(),
                        Address = potentialCustomer.Address,
                        City = potentialCustomer.City,
                        State = potentialCustomer.State,
                        ZipCode = potentialCustomer.ZipCode,
                        Country = potentialCustomer.Country,
                        CompanyName = potentialCustomer.CompanyName,
                        PhoneNumber = potentialCustomer.PhoneNumber,
                        UserId = potentialCustomer.UserId,
                        Roles = selectListItems
                    };

                    return View("EditUser", viewModel);
                }
            }
            else if (user.Roles == "Employee")
            {
                var potentialEmployee = _context.Employees.Where(e => e.UserId == user.Id).FirstOrDefault();

                if (potentialEmployee != null)
                {
                    AdminEditViewModel viewModel = new()
                    {

                        FirstName = potentialEmployee.FirstName,
                        LastName = potentialEmployee.LastName,
                        EmailAddress = potentialEmployee.EmailAddress,
                        DateOfBirth = potentialEmployee.DateOfBirth.ToString(),
                        Address = potentialEmployee.Address,
                        City = potentialEmployee.City,
                        State = potentialEmployee.State,
                        ZipCode = potentialEmployee.ZipCode,
                        Country = potentialEmployee.Country,
                        CompanyName = "NexusConnect",
                        PhoneNumber = potentialEmployee.PhoneNumber,
                        UserId = potentialEmployee.UserId,
                        Roles = selectListItems
                    };

                    return View("EditUser", viewModel);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AdminEditViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId);

            if (user == null)
            {
                return NotFound();
            }

            viewModel.Roles = new List<SelectListItem> 
            { 
                new SelectListItem 
                { 
                    Value = viewModel.SelectedRole, 
                    Text = viewModel.SelectedRole 
                } 
            };

            if (ModelState.IsValid)
            {
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Email = viewModel.EmailAddress;
                user.DateOfBirth = Convert.ToDateTime(viewModel.DateOfBirth);

                if (string.IsNullOrEmpty(viewModel.SelectedRole))
                {
                    ModelState.AddModelError("", "Please select a role");
                    return View("EditUser", viewModel);
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
                    return View("EditUser", viewModel);
                }

                await AdvanceUser(user.Id);

                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong...");
                return View("EditUser", viewModel);
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

            return View("Index", newModel);
        }

        private async Task AdvanceUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userCurrentRole = await _userManager.GetRolesAsync(user);
            var potentialProspect = _context.Prospects.Where(p => p.UserId == user.Id).FirstOrDefault();

            if (user == null || potentialProspect == null)
            {
                throw new Exception("User or prospect is null");
            }

            switch (userCurrentRole[0])
            {
                case "Customer":
                    var potentialCustomer = _context.Customers.Where(c => c.UserId == user.Id).FirstOrDefault();

                    if (potentialCustomer == null)
                    {
                        CustomerInfo customer = new()
                        {
                            FirstName = potentialProspect.FirstName,
                            LastName = potentialProspect.LastName,
                            EmailAddress = potentialProspect.EmailAddress,
                            DateOfBirth = potentialProspect.DateOfBirth,
                            Address = potentialProspect.Address,
                            City = potentialProspect.City,
                            State = potentialProspect.State,
                            ZipCode = potentialProspect.ZipCode,
                            Country = potentialProspect.Country,
                            CompanyName = potentialProspect.CompanyName,
                            PhoneNumber = potentialProspect.PhoneNumber,
                            CompanyId = potentialProspect.CompanyId,
                            UserId = potentialProspect.UserId
                        };

                        _context.Add(customer);
                        _context.SaveChanges();
                    }
                    break;
                case "Employee":
                    var potentialEmployee = _context.Employees.Where(e => e.UserId == user.Id).FirstOrDefault();

                    if (potentialEmployee == null)
                    {
                        EmployeeInfo employee = new()
                        {
                            FirstName = potentialProspect.FirstName,
                            LastName = potentialProspect.LastName,
                            EmailAddress = potentialProspect.EmailAddress,
                            DateOfBirth = potentialProspect.DateOfBirth,
                            Address = potentialProspect.Address,
                            City = potentialProspect.City,
                            State = potentialProspect.State,
                            ZipCode = potentialProspect.ZipCode,
                            PhoneNumber = potentialProspect.PhoneNumber,
                            Country = potentialProspect.Country,
                            Department = string.Empty,
                            UserId = potentialProspect.UserId
                        };

                        _context.Add(employee);
                        _context.SaveChanges();
                    }
                    break;
                default:
                    break;
            }
        }

        public async Task<IActionResult> ViewProspects()
        {
            var users = await _context.Prospects.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            AdminListProspectsViewModel viewModel = new()
            {
                Users = users
            };

            return View("ViewProspects", viewModel);
        }

        public async Task<IActionResult> ViewCustomers()
        {
            var users = await _context.Customers.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            AdminListCustomersViewModel viewModel = new()
            {
                Users = users
            };

            return View("ViewCustomers", viewModel);
        }

        public async Task<IActionResult> ViewCompanies()
        {
            var companies = await _context.Companies.ToListAsync();

            if (companies == null)
            {
                return NotFound();
            }

            AdminListCompaniesViewModel viewModel = new()
            {
                Companies = companies
            };

            return View("ViewCompanies", viewModel);
        }
    }
}
