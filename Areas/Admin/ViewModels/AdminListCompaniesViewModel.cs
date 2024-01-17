using NexusConnectCRM.Data.Models.Company;

namespace NexusConnectCRM.Areas.Admin.ViewModels
{
    public class AdminListCompaniesViewModel
    {
        public List<CompanyInfo> Companies { get; set; }
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCompanies { get; set; }

        public AdminListCompaniesViewModel()
        {
        }
    }
}
