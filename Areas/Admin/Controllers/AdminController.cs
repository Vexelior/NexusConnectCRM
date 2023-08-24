using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Admin.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Employee;
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

            var prospect = _context.Prospects.Where(p => p.UserId == user.Id).FirstOrDefault();
            var customer = _context.Customers.Where(c => c.UserId == user.Id).FirstOrDefault();
            var employee = _context.Employees.Where(e => e.UserId == user.Id).FirstOrDefault();
            var admin = _context.Users.Where(a => a.Roles == "Admin" && a.Id == user.Id).FirstOrDefault();

            if (customer != null)
            {
                AdminEditViewModel customerViewModel = new()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = DateTime.Parse(user.DateOfBirth.ToString()).ToString("MM/dd/yyyy"),
                    Roles = selectListItems,
                    Address = customer.Address,
                    City = customer.City,
                    State = customer.State,
                    ZipCode = customer.ZipCode,
                    PhoneNumber = customer.PhoneNumber,
                    Country = customer.Country,
                    EmailAddress = customer.EmailAddress,
                    CompanyName = customer.CompanyName,
                };

                return View("~/Areas/Admin/Views/Admin/EditUser.cshtml", customerViewModel);
            }
            else if (prospect != null)
            {
                AdminEditViewModel prospectViewModel = new()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = DateTime.Parse(user.DateOfBirth.ToString()).ToString("MM/dd/yyyy"),
                    Roles = selectListItems,
                    Address = prospect.Address,
                    City = prospect.City,
                    State = prospect.State,
                    ZipCode = prospect.ZipCode,
                    PhoneNumber = prospect.PhoneNumber,
                    Country = prospect.Country,
                    EmailAddress = prospect.EmailAddress,
                    CompanyName = prospect.CompanyName,
                };

                return View("~/Areas/Admin/Views/Admin/EditUser.cshtml", prospectViewModel);
            }
            else if (employee != null)
            {
                AdminEditViewModel employeeViewModel = new()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = DateTime.Parse(user.DateOfBirth.ToString()).ToString("MM/dd/yyyy"),
                    Roles = selectListItems,
                    Address = employee.Address,
                    City = employee.City,
                    State = employee.State,
                    ZipCode = employee.ZipCode,
                    PhoneNumber = employee.PhoneNumber,
                    Country = employee.Country,
                    EmailAddress = employee.EmailAddress,
                    CompanyName = "NexusConnect",
                };

                return View("~/Areas/Admin/Views/Admin/EditUser.cshtml", employeeViewModel);
            }
            else
            {
                return NotFound();
            }
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
                user.Email = viewModel.EmailAddress;
                user.DateOfBirth = Convert.ToDateTime(viewModel.DateOfBirth);

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

                await AdvanceUser(user.Id, viewModel.SelectedRole);
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

            return View("~/Areas/Admin/Views/Admin/Index.cshtml", newModel);
        }

        private async Task AdvanceUser(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userCurrentRole = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                return;
            }

            switch (role)
            {
                case "Customer":
                    var potentialCustomer = _context.Customers.Where(c => c.UserId == user.Id).FirstOrDefault();

                    if (potentialCustomer == null)
                    {
                        var potentialEmployeeToCustomer = _context.Employees.Where(e => e.UserId == user.Id).FirstOrDefault();
                        var potentialProspectToCustomer = _context.Prospects.Where(p => p.UserId == user.Id).FirstOrDefault();

                        if (potentialEmployeeToCustomer != null && potentialProspectToCustomer == null)
                        {
                            CustomerInfo customer = new()
                            {
                                FirstName = potentialEmployeeToCustomer.FirstName,
                                LastName = potentialEmployeeToCustomer.LastName,
                                EmailAddress = potentialEmployeeToCustomer.EmailAddress,
                                DateOfBirth = potentialEmployeeToCustomer.DateOfBirth,
                                Address = potentialEmployeeToCustomer.Address,
                                City = potentialEmployeeToCustomer.City,
                                State = potentialEmployeeToCustomer.State,
                                ZipCode = potentialEmployeeToCustomer.ZipCode,
                                PhoneNumber = potentialEmployeeToCustomer.PhoneNumber,
                                Country = potentialEmployeeToCustomer.Country,
                                CompanyName = "",
                                UserId = potentialEmployeeToCustomer.UserId
                            };

                            _context.Add(customer);
                            _context.Employees.Remove(potentialEmployeeToCustomer);
                            await _context.SaveChangesAsync();
                        }

                        if (potentialProspectToCustomer != null && potentialEmployeeToCustomer == null)
                        {
                            CustomerInfo customer = new()
                            {
                                FirstName = potentialProspectToCustomer.FirstName,
                                LastName = potentialProspectToCustomer.LastName,
                                EmailAddress = potentialProspectToCustomer.EmailAddress,
                                DateOfBirth = potentialProspectToCustomer.DateOfBirth,
                                Address = potentialProspectToCustomer.Address,
                                City = potentialProspectToCustomer.City,
                                State = potentialProspectToCustomer.State,
                                ZipCode = potentialProspectToCustomer.ZipCode,
                                PhoneNumber = potentialProspectToCustomer.PhoneNumber,
                                Country = potentialProspectToCustomer.Country,
                                CompanyName = potentialProspectToCustomer.CompanyName,
                                CompanyId = potentialProspectToCustomer.CompanyId,
                                UserId = potentialProspectToCustomer.UserId
                            };

                            _context.Add(customer);
                            _context.Prospects.Remove(potentialProspectToCustomer);
                            await _context.SaveChangesAsync();
                        }
                    }
                    break;
                case "Employee":
                    var potentialEmployee = _context.Employees.Where(e => e.UserId == user.Id).FirstOrDefault();

                    if (potentialEmployee == null)
                    {
                        var potentialCustomerToEmployee = _context.Customers.Where(c => c.UserId == user.Id).FirstOrDefault();
                        var potentialProspectToEmployee = _context.Prospects.Where(p => p.UserId == user.Id).FirstOrDefault();

                        if (potentialCustomerToEmployee != null && potentialProspectToEmployee == null)
                        {
                            EmployeeInfo employee = new()
                            {
                                FirstName = potentialCustomerToEmployee.FirstName,
                                LastName = potentialCustomerToEmployee.LastName,
                                EmailAddress = potentialCustomerToEmployee.EmailAddress,
                                DateOfBirth = potentialCustomerToEmployee.DateOfBirth,
                                Address = potentialCustomerToEmployee.Address,
                                City = potentialCustomerToEmployee.City,
                                State = potentialCustomerToEmployee.State,
                                ZipCode = potentialCustomerToEmployee.ZipCode,
                                Country = potentialCustomerToEmployee.Country,
                                PhoneNumber = potentialCustomerToEmployee.PhoneNumber,
                                Department = "",
                                UserId = potentialCustomerToEmployee.UserId
                            };

                            _context.Add(employee);
                            _context.Remove(potentialCustomerToEmployee);
                            await _context.SaveChangesAsync();
                        }
                        else if (potentialProspectToEmployee != null && potentialCustomerToEmployee == null)
                        {
                            EmployeeInfo employee = new()
                            {
                                FirstName = potentialProspectToEmployee.FirstName,
                                LastName = potentialProspectToEmployee.LastName,
                                EmailAddress = potentialProspectToEmployee.EmailAddress,
                                DateOfBirth = potentialProspectToEmployee.DateOfBirth,
                                Address = potentialProspectToEmployee.Address,
                                City = potentialProspectToEmployee.City,
                                State = potentialProspectToEmployee.State,
                                ZipCode = potentialProspectToEmployee.ZipCode,
                                Country = potentialProspectToEmployee.Country,
                                PhoneNumber = potentialProspectToEmployee.PhoneNumber,
                                Department = "",
                                UserId = potentialProspectToEmployee.UserId
                            };

                            _context.Add(employee);
                            _context.Remove(potentialProspectToEmployee);
                            await _context.SaveChangesAsync();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
