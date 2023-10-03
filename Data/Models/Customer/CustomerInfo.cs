using NexusConnectCRM.Data.Models.Misc;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Customer
{
    public class CustomerInfo : User
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }

        public CustomerInfo() { }
    }
}
