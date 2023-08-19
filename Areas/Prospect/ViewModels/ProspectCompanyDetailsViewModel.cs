using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Areas.Prospect.ViewModels
{
    [Keyless]
    public class ProspectCompanyDetailsViewModel
    {
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string ZipCode { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Industry { get; set; }

        public ProspectCompanyDetailsViewModel()
        {
        }

        public ProspectCompanyDetailsViewModel(ApplicationUser user)
        {
            UserId = user.Id;
        }
    }
}
