using Microsoft.AspNetCore.Identity;

namespace NexusConnectCRM.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Roles { get; set; }
        public int CompanyId { get; set; }
    }
}
