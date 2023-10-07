using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Areas.Prospect.ViewModels
{
    [Keyless]
    public class ProspectCompanyDetailsViewModel
    {
        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public List<SelectListItem> ListOfCountries { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string SelectedCountry { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public List<SelectListItem> ListOfStates { get; set; }
        [Required]
        [Display(Name = "State")]
        public string SelectedState { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        public string Zip { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Required]
        public string Website { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Industry { get; set; }


        public ProspectCompanyDetailsViewModel() { }
    }
}
