using NexusConnectCRM.Data.Models.Misc;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Prospect
{
    public class ProspectInfo : User
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }

        public ProspectInfo() { }
    }
}
