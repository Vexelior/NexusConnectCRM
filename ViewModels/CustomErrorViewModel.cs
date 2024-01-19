using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusConnectCRM.ViewModels
{
    [Keyless]
    [NotMapped]
    public class CustomErrorViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}
