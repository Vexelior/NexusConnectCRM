using NexusConnectCRM.Data.Models.Help;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusConnectCRM.Areas.Employee.ViewModels
{
    public class HelpEditViewModel
    {
        public int Id { get; set; }
        public HelpInfo Help { get; set; }
        [Required(ErrorMessage = "Please enter a response.")]
        public string Response { get; set; }
        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile Image { get; set; }
        public List<HelpResponseInfo> HelpResponses { get; set; }
        public string Responder { get; set; }
        public HelpEditViewModel() { }
    }
}
