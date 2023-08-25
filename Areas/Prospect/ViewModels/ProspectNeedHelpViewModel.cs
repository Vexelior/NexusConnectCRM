using NexusConnectCRM.Data.Models.Prospect;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Areas.Prospect.ViewModels
{
    public class ProspectNeedHelpViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public string Author { get; set; }
        public string UserId { get; set; }
        

        public ProspectNeedHelpViewModel()
        {
        }

        public ProspectNeedHelpViewModel(ProspectInfo info)
        {
            UserId = info.UserId;
            Author = info.FirstName + " " + info.LastName;
        }
    }
}
