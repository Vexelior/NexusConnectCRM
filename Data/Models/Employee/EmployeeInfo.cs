using NexusConnectCRM.Data.Models.Misc;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Data.Models.Employee
{
    public class EmployeeInfo : User
    {
        [Key]
        public int Id { get; set; }
        public string Department { get; set; }
    }
}
