using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Help
{
    public class HelpResponseInfo
    {
        [Key]
        public int Id { get; set; }
        public string ResponseId { get; set; }
        public string Response { get; set; }
        public byte[] Image { get; set; }
        public string Author { get; set; }
        public bool IsEmployee { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public HelpResponseInfo() { }
    }
}
