using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Areas.Prospect.ViewModels
{
    [Keyless]
    public class ProspectUserDetailsViewModel
    {
        public string UserId { get; set; }
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

        public ProspectUserDetailsViewModel()
        {
        }

        public ProspectUserDetailsViewModel(ProspectInfo user)
        {
            UserId = user.UserId;
        }
    }
}
