using Microsoft.AspNetCore.Identity;

namespace NexusConnectCRM.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CompanyName { get; set; }
        public string Roles { get; set; }
        public int CompanyId { get; set; }
    }
}
