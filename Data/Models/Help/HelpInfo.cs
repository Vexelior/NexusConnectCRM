﻿using System.ComponentModel;
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
        [Display(Name = "Pending")]
        public bool IsPending { get; set; }
        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }
        [Display(Name = "Rejected")]
        public bool IsRejected { get; set; }
        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool EmployeeViewed { get; set; }
        public bool CustomerViewed { get; set; }
        public bool CustomerWasRecentResponse { get; set; }
        public bool EmployeeWasRecentResponse { get; set; }

        public List<HelpResponseInfo> HelpResponses { get; set; }
    }
}
