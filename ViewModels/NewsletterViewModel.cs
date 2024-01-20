using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusConnectCRM.ViewModels
{
    [Keyless]
    [NotMapped]
    public class NewsletterViewModel
    {
        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 2)]
        public string Email { get; set; }
    }
}
