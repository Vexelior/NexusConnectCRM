using Microsoft.AspNetCore.Identity;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Admin.ViewModels
{
    public class AdminIndexViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public int TotalUsers { get; set; }
    }
}
