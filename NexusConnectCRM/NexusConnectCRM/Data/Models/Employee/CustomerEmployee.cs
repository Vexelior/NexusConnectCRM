using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Employee
{
    public class CustomerEmployee
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
    }
}
