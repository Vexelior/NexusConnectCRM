using NexusConnectCRM.Data.Models.Help;

namespace NexusConnectCRM.ViewModels.HelpInfoData
{
    public class HelpInfoIndexViewModel
    {
        public int NewResponses { get; set; }
        public List<HelpInfo> HelpInfos { get; set; }

        public HelpInfoIndexViewModel() { }
    }
}
