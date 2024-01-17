using NexusConnectCRM.Data.Models.Customer;

namespace NexusConnectCRM.Areas.Admin.ViewModels
{
    public class AdminListCustomersViewModel
    {
        public List<CustomerInfo> Customers { get; set; }
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCustomers { get; set; }

        public AdminListCustomersViewModel()
        {
        }
    }
}
