using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusConnectCRM.ViewModels
{
    [Keyless]
    [NotMapped]
    public class ContactViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50, MinimumLength = 2)]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Subject { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Message { get; set; }
    }
}
