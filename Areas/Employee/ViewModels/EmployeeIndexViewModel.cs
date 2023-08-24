using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Areas.Employee.ViewModels
{
    public class EmployeeIndexViewModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UncompletedTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalTasks { get; set; }

        public EmployeeIndexViewModel() { }
    }
}
