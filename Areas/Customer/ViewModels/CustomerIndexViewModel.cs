using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Customer.ViewModels
{
    public class CustomerIndexViewModel
    {
        public CustomerInfo Customer { get; set; }
        public CompanyInfo Company { get; set; }
        public string Role { get; set; }
    }
}
