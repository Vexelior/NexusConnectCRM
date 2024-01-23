using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        [DisplayName("Upload Image")]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public string Author { get; set; }
        public string AuthorName { get; set; }
        public bool IsPending { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool EmployeeViewed { get; set; }
        public bool CustomerViewed { get; set; }
        public bool CustomerWasRecentResponse { get; set; }
        public bool EmployeeWasRecentResponse { get; set; }

        public List<HelpResponseInfo> HelpResponses { get; set; }
    }
}
