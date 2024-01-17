using NexusConnectCRM.Data.Models.Prospect;

namespace NexusConnectCRM.Areas.Admin.ViewModels
{
    public class AdminListProspectsViewModel
    {
        public List<ProspectInfo> Prospects { get; set; }
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalProspects { get; set; }
    }
}
