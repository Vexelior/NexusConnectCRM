using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Help;

namespace NexusConnectCRM.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            UserManager<ApplicationUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            if (!context.Roles.Any())
            {
                List<IdentityRole> roles =
                [
                        new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                        new IdentityRole { Name = "Customer", NormalizedName = "USER" },
                        new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" },
                        new IdentityRole { Name = "Prospect", NormalizedName = "PROSPECT" }
                    ];

                foreach (IdentityRole role in roles)
                {
                    IdentityResult result = roleManager.CreateAsync(role).Result;

                    if (!result.Succeeded)
                    {
                        throw new Exception($"Error creating role '{role.Name}'.");
                    }
                    else
                    {
                        Console.WriteLine($"Created role '{role.Name}'.");
                    }
                }
            }

            if (!context.Users.Any())
            {
                // Create Admin \\
                ApplicationUser adminUser = new() { 
                    UserName = "admin@mail.com", 
                    Email = "admin@mail.com", 
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User",
                    Roles = "Admin"
                };
                IdentityResult adminResult = userManager.CreateAsync(adminUser, "Pass1234!").Result;

                if (adminResult.Succeeded)
                {
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
                else
                {
                    throw new Exception("Admin account creation failed.");
                }

                // Create Customer \\
                ApplicationUser customerUser = new() { 
                    UserName = "customer@mail.com", 
                    Email = "customer@mail.com", 
                    EmailConfirmed = true,
                    FirstName = "Customer",
                    LastName = "User",
                    Roles = "Customer"
                };
                IdentityResult customerResult = userManager.CreateAsync(customerUser, "Pass1234!").Result;

                if (customerResult.Succeeded)
                {
                    userManager.AddToRoleAsync(customerUser, "Customer").Wait();
                }
                else
                {
                    throw new Exception("Customer account creation failed.");
                }

                // Create Employee \\
                ApplicationUser employeeUser = new() { 
                    UserName = "employee@mail.com", 
                    Email = "employee@mail.com", 
                    EmailConfirmed = true,
                    FirstName = "Employee",
                    LastName = "User",
                    Roles = "Employee"
                };
                IdentityResult employeeResult = userManager.CreateAsync(employeeUser, "Pass1234!").Result;

                if (employeeResult.Succeeded)
                {
                    userManager.AddToRoleAsync(employeeUser, "Employee").Wait();
                }
                else
                {
                    throw new Exception("Employee account creation failed.");
                }

                // Create Prospect \\
                ApplicationUser prospectUser = new() { 
                    UserName = "prospect@mail.com", 
                    Email = "prospect@mail.com", 
                    EmailConfirmed = true,
                    FirstName = "Prospect",
                    LastName = "User",
                    Roles = "Prospect"
                };
                IdentityResult prospectResult = userManager.CreateAsync(prospectUser, "Pass1234!").Result;

                if (prospectResult.Succeeded)
                {
                    userManager.AddToRoleAsync(prospectUser, "Prospect").Wait();
                }
                else
                {
                    throw new Exception("Prospect account creation failed.");
                }
            }

            //if (!context.Prospects.Any())
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        ProspectInfo prospectInfo = new()
            //        {
            //            FirstName = "Prospect",
            //            LastName = $"User{i}",
            //            EmailAddress = $"prospect{i}@mail.com",
            //            DateOfBirth = DateTime.Now,
            //            Address = $"123 Prospect St, Unit {i}",
            //            City = $"Prospect City {i}",
            //            State = $"Prospect State {i}",
            //            ZipCode = $"1234{i}",
            //            Country = $"Prospect Country {i}",
            //            CompanyName = $"Company{i}",
            //            PhoneNumber = $"555-555-555{i}",
            //            IsActive = true,
            //            IsContacted = false,
            //            CreatedDate = DateTime.Now,
            //            ModifiedDate = DateTime.Now
            //        };

            //        context.Prospects.Add(prospectInfo);
            //        context.SaveChanges();
            //    }
            //}

            //if (!context.Companies.Any())
            //{
            //    for (int i = 0; i < 22; i++)
            //    {
            //        CompanyInfo companyInfo = new()
            //        {
            //            Name = $"Company{i}",
            //            Address = $"123 Company St, Unit {i}",
            //            City = $"Company City {i}",
            //            State = $"Company State {i}",
            //            Zip = $"1234{i}",
            //            Country = $"Company Country {i}",
            //            Phone = $"555-555-555{i}",
            //            Website = $"www.company{i}.com",
            //            Email = $"company{i}@mail.com",
            //            Industry = $"Company Industry {i}",
            //            NeedsContact = true
            //        };

            //        context.Companies.Add(companyInfo);
            //        context.SaveChanges();
            //    }
            //}

            //if (!context.Customers.Any())
            //{
            //    for (int i = 0; i < 42; i++)
            //    {
            //        CustomerInfo customer = new()
            //        {
            //            FirstName = "Customer",
            //            LastName = $"Customer {i}",
            //            EmailAddress = $"Customer{i}@mail.com",
            //            DateOfBirth = DateTime.Now,
            //            Address = $"123 Prospect St, Unit {i}",
            //            City = $"Customer City {i}",
            //            State = $"Customer State {i}",
            //            ZipCode = $"1234{i}",
            //            Country = $"Customer Country {i}",
            //            CompanyName = $"Customer{i}",
            //            PhoneNumber = $"555-555-555{i}",
            //            IsActive = true,
            //            CreatedDate = DateTime.Now,
            //            ModifiedDate = DateTime.Now,
            //            NeedsContact = true
            //        };

            //        context.Customers.Add(customer);
            //        context.SaveChanges();
            //    }
            //}

            // My testing account. \\
            if (context.Users.Where(u => u.Email == "asanderson1994s@gmail.com").FirstOrDefault() == null)
            {
                ApplicationUser testUser = new()
                {
                    UserName = "asanderson1994s@gmail.com",
                    Email = "asanderson1994s@gmail.com",
                    NormalizedEmail = "ASANDERSON1994S@GMAIL.COM",
                    EmailConfirmed = true,
                    FirstName = "Alexander",
                    LastName = "Sanderson",
                    Roles = "Prospect",
                    NormalizedUserName = "ASANDERSON1994S@GMAIL.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false
                };
                IdentityResult accountResult = userManager.CreateAsync(testUser, "Pass1234!").Result;

                if (accountResult.Succeeded)
                {
                    userManager.AddToRoleAsync(testUser, "Prospect").Wait();
                }
                else
                {
                    throw new Exception("Test account creation failed");
                }

                CompanyInfo company = new()
                { 
                    Name = "The Tech Academy",
                    Address = "310 SW 4th Ave, Suite 200",
                    City = "Portland",
                    State = "Oregon",
                    Zip = "97204",
                    Country = "United States",
                    Phone = "503-206-6915",
                    Website = "https://www.learncodinganywhere.com",
                    Email = "info@learncodinganywhere.com",
                    Industry = "Education",
                    NeedsContact = false
                };

                context.Companies.Add(company);
                context.SaveChanges();

                var userId = context.Users.Where(u => u.Email == "asanderson1994s@gmail.com").FirstOrDefault().Id;
                var techAcademyId = context.Companies.Where(c => c.Name == "The Tech Academy").FirstOrDefault().Id;

                ProspectInfo prospectInfo = new()
                {
                    FirstName = "Alexander",
                    LastName = "Sanderson",
                    EmailAddress = "asanderson1994s@gmail.com",
                    DateOfBirth = new DateTime(1994, 8, 19),
                    Address = "4602 Boise Ct",
                    City = "Vancouver",
                    State = "Washington",
                    ZipCode = "98661",
                    Country = "United States",
                    CompanyName = "The Tech Academy",
                    PhoneNumber = "360-281-9758",
                    UserId = userId,
                    CompanyId = techAcademyId,
                    IsActive = true,
                    IsContacted = null,
                    IsHelped = null,
                    NeedsHelp = null,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                context.Prospects.Add(prospectInfo);
                context.SaveChanges();
            }

            if (!context.Help.Any())
            {
                List<HelpInfo> helpInfos = [];
                string testAccountUserId = context.Users.Where(u => u.Email == "asanderson1994s@gmail.com").FirstOrDefault().Id;

                for (int i = 1; i <= 100; i++)
                {
                    HelpInfo helpInfo = new()
                    {
                        Title = $"Help Title {i}",
                        Description = $"Help Description {i}",
                        Author = testAccountUserId,
                        IsPending = true,
                        IsApproved = false,
                        IsRejected = false,
                        IsCompleted = false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        EmployeeViewed = false,
                        CustomerViewed = true,
                        CustomerWasRecentResponse = true,
                        EmployeeWasRecentResponse = false
                    };

                    helpInfos.Add(helpInfo);
                }

                context.Help.AddRange(helpInfos);
                context.SaveChanges();
            }

            return app;
        }
    }
}
