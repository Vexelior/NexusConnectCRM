using Microsoft.AspNetCore.Identity;

namespace NexusConnectCRM.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Roles { get; set; }
        public int CompanyId { get; set; }
    }
}
