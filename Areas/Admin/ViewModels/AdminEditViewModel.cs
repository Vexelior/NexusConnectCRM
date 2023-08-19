using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Areas.Admin.ViewModels
{
    [Keyless]
    public class AdminEditViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Mailing Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        [Display(Name = "Postal Code")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string CompanyName { get; set; }
        public string SelectedRole { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public AdminEditViewModel()
        {
        }

        public AdminEditViewModel(ApplicationUser user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            DateOfBirth = Convert.ToDateTime(user.DateOfBirth).ToString("MM/dd/yyyy");
            PhoneNumber = user.PhoneNumber;
            Address = user.Address;
            City = user.City;
            State = user.State;
            ZipCode = user.ZipCode;
            Country = user.Country;
            CompanyName = user.CompanyName;
        }
    }
}
