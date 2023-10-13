using NexusConnectCRM.Data.Models.Help;

namespace NexusConnectCRM.Areas.Employee.ViewModels
{
    public class ListHelpViewModel
    {
        public List<HelpInfo> HelpList { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }

        public ListHelpViewModel() { }
    }
}
