using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Customer.ViewModels
{
    public class CustomerIndexViewModel
    {
       public ApplicationUser User { get; set; }
       public CustomerInfo CustomerInfo { get; set; }
       public CompanyInfo Company { get; set; }
    }
}
