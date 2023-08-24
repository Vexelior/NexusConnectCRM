using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Employee;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;
using NexusConnectCRM.Areas.Admin.ViewModels;
using NexusConnectCRM.Data.Models.Company;

namespace NexusConnectCRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=NexusConnectCRM;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        public DbSet<ProspectInfo> Prospects { get; set; } = null!;
        public DbSet<CustomerInfo> Customers { get; set; } = null!;
        public DbSet<EmployeeInfo> Employees { get; set; } = null!;
        public DbSet<ProspectEmployee> ProspectEmployees { get; set; } = null!;
        public DbSet<CustomerEmployee> CustomerEmployees { get; set; } = null!;
        public DbSet<CompanyInfo> Companies { get; set; } = null!;
    }
}