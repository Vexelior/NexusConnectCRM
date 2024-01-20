using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Newsletter
{
    public class Newsletter
    {
        [Key]
        public int SubscriberId { get; set; }
        public string Email { get; set; }
        public bool IsSubscribed { get; set; }
        public string UserId { get; set; }
    }
}
