using NexusConnectCRM.Data.Models.Help;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Areas.Employee.ViewModels
{
    public class ListHelpViewModel
    {
        public List<HelpInfo> HelpList { get; set; }
        public string SearchQuery { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalHelp { get; set; }

        // Radio Button Filters
        [Display(Name = "All")]
        public bool FilterAll { get; set; }
        [Display(Name = "Approved")]
        public bool FilterApproved { get; set; }
        [Display(Name = "Pending")]
        public bool FilterPending { get; set; }
        [Display(Name = "Rejected")]
        public bool FilterRejected { get; set; }
        [Display(Name = "Completed")]
        public bool FilterCompleted { get; set; }

        public ListHelpViewModel() { }
    }
}
