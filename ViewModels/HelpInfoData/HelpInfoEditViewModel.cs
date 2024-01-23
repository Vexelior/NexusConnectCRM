using NexusConnectCRM.Data.Models.Help;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusConnectCRM.ViewModels.HelpInfoData
{
    public class HelpInfoEditViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public HelpInfo Help { get; set; }
        [Required]
        public string Response { get; set; }
        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile Image { get; set; }
        public List<HelpResponseInfo> HelpResponses { get; set; }
        public HelpInfoEditViewModel() { }
    }
}
