using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Employee
{
    public class ProspectEmployee
    {
        [Key]
        public int Id { get; set; }
        public int ProspectId { get; set; }
        public int EmployeeId { get; set; }
    }
}
