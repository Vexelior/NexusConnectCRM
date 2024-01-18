using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Admin.ViewModels
{
    public class AdminListUsersViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalUsers { get; set; }

        public AdminListUsersViewModel()
        {
        }
    }
}
