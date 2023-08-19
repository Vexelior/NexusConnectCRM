using NexusConnectCRM.Data.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace NexusConnectCRM.Areas.Employee.ViewModels
{
    public class ListCustomersViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
