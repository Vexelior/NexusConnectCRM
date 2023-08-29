using NexusConnectCRM.Data.Models.Help;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.ViewModels
{
    public class AuthorHelpEditViewModel
    {
        public int Id { get; set; }
        public HelpInfo Help { get; set; }
        [Required]
        public string Response { get; set; }
        public List<HelpResponseInfo> HelpResponses { get; set; }
        public AuthorHelpEditViewModel() { }
    }
}
