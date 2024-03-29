﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Employee;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Help;
using NexusConnectCRM.Data.Models.Newsletter;

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

        // Identity Tables \\
        public DbSet<ProspectInfo> Prospects { get; set; } = null!;
        public DbSet<CustomerInfo> Customers { get; set; } = null!;
        public DbSet<EmployeeInfo> Employees { get; set; } = null!;
        public DbSet<CompanyInfo> Companies { get; set; } = null!;

        // Help Tables \\
        public DbSet<HelpInfo> Help { get; set; } = null!;
        public DbSet<HelpResponseInfo> HelpFeedback { get; set; } = null!;

        // Relationship Tables \\
        public DbSet<ProspectEmployee> ProspectEmployees { get; set; } = null!;
        public DbSet<CustomerEmployee> CustomerEmployees { get; set; } = null!;

        // NewsLetter Tables \\
        public DbSet<Newsletter> Newsletters { get; set; } = null!;
    }
}