using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Help
{
    public class HelpInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string Author { get; set; }
        [Display(Name = "Pending")]
        public bool IsPending { get; set; }
        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }
        [Display(Name = "Rejected")]
        public bool IsRejected { get; set; }
        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }
    }
}
