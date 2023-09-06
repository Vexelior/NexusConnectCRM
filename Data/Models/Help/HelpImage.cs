using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusConnectCRM.Data.Models.Help
{
    public class HelpImage
    {
        [Key]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public HelpResponseInfo HelpResponse { get; set; }
        public int HelpResponseId { get; set; }
        public HelpInfo Help { get; set; }
        public int HelpId { get; set; }
        public bool IsHelpResponse { get; set; }
        public bool IsHelp { get; set; }
        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile ImageFile { get; set; }

        public HelpImage() { }
    }
}
