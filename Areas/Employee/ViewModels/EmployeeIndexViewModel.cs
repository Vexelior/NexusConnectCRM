using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Employee.ViewModels
{
    public class EmployeeIndexViewModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public EmployeeIndexViewModel() { }
    }
}
