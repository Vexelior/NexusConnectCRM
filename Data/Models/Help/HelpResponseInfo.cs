using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusConnectCRM.Data.Models.Help
{
    public class HelpResponseInfo
    {
        [Key]
        public int Id { get; set; }
        public int ResponseId { get; set; }
        public string Response { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public string Author { get; set; }
        public bool IsEmployee { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public HelpResponseInfo() { }
    }
}
