using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Help
{
    public class HelpResponseInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Response { get; set; }
        public byte[] Image { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public HelpInfo Help { get; set; }
    }
}
