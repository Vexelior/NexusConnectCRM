using Microsoft.AspNetCore.Identity;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Admin.ViewModels
{
    public class AdminIndexViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
