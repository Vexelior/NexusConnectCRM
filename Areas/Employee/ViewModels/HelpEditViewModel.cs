using Microsoft.AspNetCore.SignalR;
using NexusConnectCRM.Data.Models.Help;

namespace NexusConnectCRM.Areas.Employee.ViewModels
{
    public class HelpEditViewModel
    {
        public int Id { get; set; }
        public HelpInfo Help { get; set; }
        public string Response { get; set; }
        public List<HelpResponseInfo> HelpResponses { get; set; }

        public HelpEditViewModel() { }
    }
}
