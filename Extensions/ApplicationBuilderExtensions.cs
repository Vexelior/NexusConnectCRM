﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Prospect;
using NexusConnectCRM.Data.Models.Company;
using NexusConnectCRM.Data.Models.Customer;
using NexusConnectCRM.Data.Models.Help;
using NexusConnectCRM.Data.Models.Employee;

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
                    new IdentityRole { Name = "HeadAdmin", NormalizedName = "HEADADMIN", ConcurrencyStamp = Guid.NewGuid().ToString("D") },
                    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString("D") },
                    new IdentityRole { Name = "Customer", NormalizedName = "CUSTOMER", ConcurrencyStamp = Guid.NewGuid().ToString("D") },
                    new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE", ConcurrencyStamp = Guid.NewGuid().ToString("D") },
                    new IdentityRole { Name = "Prospect", NormalizedName = "PROSPECT", ConcurrencyStamp = Guid.NewGuid().ToString("D") },
                    new IdentityRole { Name = "Help Desk", NormalizedName = "HELP DESK", ConcurrencyStamp = Guid.NewGuid().ToString("D") }
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
                // Create HeadAdmin \\
                ApplicationUser headAdminUser = new()
                {
                    UserName = "headadmin@mail.com",
                    Email = "headadmin@mail.com",
                    PhoneNumber = "(555) 555 - 5555",
                    DateOfBirth = new DateTime(1982, 10, 22).Date,
                    EmailConfirmed = true,
                    FirstName = "HeadAdmin",
                    LastName = "User",
                    Roles = "HeadAdmin",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsOnline = false
                };
                IdentityResult headadminResult = userManager.CreateAsync(headAdminUser, "Pass1234!").Result;

                if (headadminResult.Succeeded)
                {
                    userManager.AddToRoleAsync(headAdminUser, "HeadAdmin").Wait();
                }
                else
                {
                    throw new Exception("HeadAdmin account creation failed.");
                }

                // Create Admin \\
                ApplicationUser adminUser = new()
                {
                    UserName = "admin@mail.com",
                    Email = "admin@mail.com",
                    PhoneNumber = "(555) 555 - 5555",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User",
                    Roles = "Admin",
                    DateOfBirth = new DateTime(1982, 10, 22).Date,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsOnline = false
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

                // Create Help Desk \\
                ApplicationUser helpDeskUser = new()
                {
                    UserName = "helpdesk@mail.com",
                    Email = "helpdesk@mail.com",
                    PhoneNumber = "(555) 555 - 5555",
                    DateOfBirth = new DateTime(1982, 10, 22).Date,
                    EmailConfirmed = true,
                    FirstName = "Help",
                    LastName = "Desk",
                    Roles = "Help Desk",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsOnline = false
                };
                IdentityResult helpdeskResult = userManager.CreateAsync(helpDeskUser, "Pass1234!").Result;

                if (helpdeskResult.Succeeded)
                {
                    userManager.AddToRoleAsync(helpDeskUser, "Help Desk").Wait();

                    // Create Help Desk Employee \\
                    EmployeeInfo helpDeskEmployee = new()
                    {
                        FirstName = helpDeskUser.FirstName,
                        LastName = helpDeskUser.LastName,
                        Department = "Help Desk",
                        UserId = helpDeskUser.Id,
                        EmailAddress = helpDeskUser.Email,
                        DateOfBirth = helpDeskUser.DateOfBirth.Date,
                        Address = "123 Support St",
                        City = "Portland",
                        State = "Oregon",
                        ZipCode = "97515",
                        Country = "United States",
                        PhoneNumber = helpDeskUser.PhoneNumber,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    context.Employees.Add(helpDeskEmployee);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Help Desk account creation failed.");
                }

                // Create Employee \\
                ApplicationUser employeeUser = new()
                {
                    UserName = "employee@mail.com",
                    Email = "employee@mail.com",
                    PhoneNumber = "(555) 555 - 5555",
                    DateOfBirth = new DateTime(1991, 6, 12).Date,
                    EmailConfirmed = true,
                    FirstName = "Employee",
                    LastName = "User",
                    Roles = "Employee",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsOnline = false
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
            }

            if (!context.Companies.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    CompanyInfo companyInfo = new()
                    {
                        Name = $"Company{i}",
                        Address = $"123 Company St, Unit {i}",
                        City = $"Company City {i}",
                        State = $"Company State {i}",
                        Zip = $"1234{i}",
                        Country = $"United States",
                        Phone = $"(555) 555 - 555{i}",
                        Website = $"https://www.company{i}.com",
                        Email = $"company{i}@mail.com",
                        Industry = $"Company Industry {i}",
                        NeedsContact = false
                    };

                    context.Companies.Add(companyInfo);
                    context.SaveChanges();
                }
            }

            if (!context.Prospects.Any())
            {
                int totalCompanies = context.Companies.Count();

                for (int i = 0; i < 10; i++)
                {
                    Random random = new();

                    int randomCompanyId = random.Next(1, totalCompanies);
                    CompanyInfo randomCompany = context.Companies.Where(c => c.Id == randomCompanyId).FirstOrDefault();

                    DateTime randomDateOfBirth = DateTime.Now.AddYears(-random.Next(18, 65));
                    randomDateOfBirth = randomDateOfBirth.AddMonths(-randomDateOfBirth.Month + random.Next(1, 12));
                    randomDateOfBirth = randomDateOfBirth.AddDays(-randomDateOfBirth.Day + random.Next(1, 28));

                    ProspectInfo prospectInfo = new()
                    {
                        FirstName = "Prospect",
                        LastName = $"User{i}",
                        CompanyId = randomCompany.Id,
                        EmailAddress = $"prospect{i}@mail.com",
                        DateOfBirth = randomDateOfBirth.Date,
                        Address = $"123 Prospect St, Unit {i}",
                        City = $"Prospect City {i}",
                        State = $"Prospect State {i}",
                        ZipCode = $"1234{i}",
                        Country = $"United States",
                        CompanyName = randomCompany.Name,
                        PhoneNumber = $"(555) 555 - 555{i}",
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    };

                    context.Prospects.Add(prospectInfo);

                    // Create an application user for each prospect. \\
                    ApplicationUser prospectUser = new()
                    {
                        UserName = prospectInfo.EmailAddress,
                        Email = prospectInfo.EmailAddress,
                        EmailConfirmed = true,
                        DateOfBirth = prospectInfo.DateOfBirth.Date,
                        PhoneNumber = prospectInfo.PhoneNumber,
                        FirstName = prospectInfo.FirstName,
                        LastName = prospectInfo.LastName,
                        Roles = "Prospect",
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                        LockoutEnabled = false,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        IsOnline = false
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

                    prospectInfo.UserId = prospectUser.Id;

                    context.SaveChanges();
                }
            }

            if (!context.Customers.Any())
            {
                int totalCompanies = context.Companies.Count();

                for (int i = 0; i < 10; i++)
                {
                    Random random = new();
                    int randomCompanyId = random.Next(1, totalCompanies);

                    CompanyInfo randomCompany = context.Companies.Where(c => c.Id == randomCompanyId).FirstOrDefault();

                    DateTime randomDateOfBirth = DateTime.Now.AddYears(-random.Next(18, 65));
                    randomDateOfBirth = randomDateOfBirth.AddMonths(-randomDateOfBirth.Month + random.Next(1, 12));
                    randomDateOfBirth = randomDateOfBirth.AddDays(-randomDateOfBirth.Day + random.Next(1, 28));

                    CustomerInfo customer = new()
                    {
                        FirstName = "Customer",
                        LastName = $"Customer {i}",
                        CompanyId = randomCompany.Id,
                        EmailAddress = $"Customer{i}@mail.com",
                        DateOfBirth = randomDateOfBirth.Date,
                        Address = $"123 Prospect St, Unit {i}",
                        City = $"Customer City {i}",
                        State = $"Customer State {i}",
                        ZipCode = $"1234{i}",
                        Country = $"United States",
                        CompanyName = randomCompany.Name,
                        PhoneNumber = $"(555) 555 - 555{i}",
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    };

                    context.Customers.Add(customer);

                    // Create an application user for each customer. \\
                    ApplicationUser customerUser = new()
                    {
                        UserName = customer.EmailAddress,
                        Email = customer.EmailAddress,
                        PhoneNumber = customer.PhoneNumber,
                        EmailConfirmed = true,
                        DateOfBirth = randomDateOfBirth.Date,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Roles = "Customer",
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                        LockoutEnabled = false,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        IsOnline = false
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

                    customer.UserId = customerUser.Id;

                    context.SaveChanges();
                }
            }

            // My testing account. \\
            if (context.Users.Where(u => u.Email == "asanderson1994s@gmail.com").FirstOrDefault() == null)
            {
                ApplicationUser testUser = new()
                {
                    UserName = "asanderson1994s@gmail.com",
                    Email = "asanderson1994s@gmail.com",
                    NormalizedEmail = "ASANDERSON1994S@GMAIL.COM",
                    DateOfBirth = new DateTime(1994, 8, 19).Date,
                    EmailConfirmed = true,
                    FirstName = "Alexander",
                    LastName = "Sanderson",
                    Roles = "Prospect",
                    PhoneNumber = "(360) 281 - 9758",
                    NormalizedUserName = "ASANDERSON1994S@GMAIL.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsOnline = false
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
                    Phone = "(503) 206 - 6915",
                    Website = "https://www.learncodinganywhere.com",
                    Email = "info@learncodinganywhere.com",
                    Industry = "Education",
                    NeedsContact = false
                };

                context.Companies.Add(company);
                context.SaveChanges();

                var userId = context.Users.Where(u => u.Email == testUser.Email).FirstOrDefault().Id;
                var techAcademyId = context.Companies.Where(c => c.Name == company.Name).FirstOrDefault().Id;

                ProspectInfo prospectInfo = new()
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    EmailAddress = testUser.Email,
                    DateOfBirth = testUser.DateOfBirth.Date,
                    Address = "4602 Boise Ct",
                    City = "Vancouver",
                    State = "Washington",
                    ZipCode = "98661",
                    Country = "United States",
                    CompanyName = company.Name,
                    PhoneNumber = testUser.PhoneNumber,
                    UserId = userId,
                    CompanyId = techAcademyId,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                context.Prospects.Add(prospectInfo);
                context.SaveChanges();
            }

            if (context.Users.Where(u => u.Email == "b.johnson@gmail.com").FirstOrDefault() == null)
            {
                Random random = new();

                ApplicationUser testUser = new()
                {
                    UserName = "b.johnson@gmail.com",
                    Email = "b.johnson@gmail.com",
                    NormalizedEmail = "B.JOHNSON@GMAIL.COM",
                    DateOfBirth = DateTime.Now.AddYears(-random.Next(18, 65)),
                    EmailConfirmed = true,
                    FirstName = "Bill",
                    LastName = "Johnson",
                    Roles = "Prospect",
                    PhoneNumber = "(619) 283-5240",
                    NormalizedUserName = "B.JOHNSON@GMAIL.COM",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    IsOnline = false
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
                    Name = "Google",
                    Address = "1600 Amphitheatre Parkway",
                    City = "Mountain View",
                    State = "California",
                    Zip = "94043",
                    Country = "United States",
                    Phone = "(650) 253-0000",
                    Website = "https://www.google.com",
                    Email = "support-in@google.com",
                    Industry = "Technology",
                    NeedsContact = false
                };

                context.Companies.Add(company);
                context.SaveChanges();

                var userId = context.Users.Where(u => u.Email == testUser.Email).FirstOrDefault().Id;
                var techAcademyId = context.Companies.Where(c => c.Name == company.Name).FirstOrDefault().Id;

                ProspectInfo prospectInfo = new()
                {
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    EmailAddress = testUser.Email,
                    DateOfBirth = testUser.DateOfBirth.Date,
                    Address = "123 Main St",
                    City = "San Diego",
                    State = "California",
                    ZipCode = "91941",
                    Country = "United States",
                    CompanyName = company.Name,
                    PhoneNumber = testUser.PhoneNumber,
                    UserId = userId,
                    CompanyId = techAcademyId,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                context.Prospects.Add(prospectInfo);
                context.SaveChanges();
            }

            if (!context.Help.Any())
            {
                List<HelpInfo> helpInfos1 = [];
                string testAccountUserId1 = context.Users.Where(u => u.Email == "asanderson1994s@gmail.com").FirstOrDefault().Id;
                string testAccountName1 = context.Prospects.Where(p => p.UserId == testAccountUserId1).Select(p => p.FirstName + " " + p.LastName).FirstOrDefault();

                for (int i = 1; i <= 20; i++)
                {
                    HelpInfo pendingHelpInfo = new()
                    {
                        Title = $"Help Title {i}",
                        Description = $"Help Description {i}",
                        Author = testAccountUserId1,
                        AuthorName = testAccountName1,
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

                    helpInfos1.Add(pendingHelpInfo);

                    HelpInfo approvedHelpInfo = new()
                    {
                        Title = $"Help Title {i}",
                        Description = $"Help Description {i}",
                        Author = testAccountUserId1,
                        AuthorName = testAccountName1,
                        IsPending = false,
                        IsApproved = true,
                        IsRejected = false,
                        IsCompleted = false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        EmployeeViewed = false,
                        CustomerViewed = true,
                        CustomerWasRecentResponse = true,
                        EmployeeWasRecentResponse = false
                    };

                    helpInfos1.Add(approvedHelpInfo);

                    HelpInfo rejectedHelpInfo = new()
                    {
                        Title = $"Help Title {i}",
                        Description = $"Help Description {i}",
                        Author = testAccountUserId1,
                        AuthorName = testAccountName1,
                        IsPending = false,
                        IsApproved = false,
                        IsRejected = true,
                        IsCompleted = false,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        EmployeeViewed = false,
                        CustomerViewed = true,
                        CustomerWasRecentResponse = true,
                        EmployeeWasRecentResponse = false
                    };

                    helpInfos1.Add(rejectedHelpInfo);

                    HelpInfo completedHelpInfo = new()
                    {
                        Title = $"Help Title {i}",
                        Description = $"Help Description {i}",
                        Author = testAccountUserId1,
                        AuthorName = testAccountName1,
                        IsPending = false,
                        IsApproved = false,
                        IsRejected = false,
                        IsCompleted = true,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        EmployeeViewed = false,
                        CustomerViewed = true,
                        CustomerWasRecentResponse = true,
                        EmployeeWasRecentResponse = false
                    };

                    helpInfos1.Add(completedHelpInfo);
                }

                context.Help.AddRange(helpInfos1);
                context.SaveChanges();
            }

            List<HelpInfo> helpInfos2 = [];
            string testAccountUserId2 = context.Users.Where(u => u.Email == "b.johnson@gmail.com").FirstOrDefault().Id;
            string testAccountName2 = context.Prospects.Where(p => p.UserId == testAccountUserId2).Select(p => p.FirstName + " " + p.LastName).FirstOrDefault();

            for (int i = 1; i <= 20; i++)
            {
                HelpInfo pendingHelpInfo = new()
                {
                    Title = $"Help Title {i}",
                    Description = $"Help Description {i}",
                    Author = testAccountUserId2,
                    AuthorName = testAccountName2,
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

                helpInfos2.Add(pendingHelpInfo);

                HelpInfo approvedHelpInfo = new()
                {
                    Title = $"Help Title {i}",
                    Description = $"Help Description {i}",
                    Author = testAccountUserId2,
                    AuthorName = testAccountName2,
                    IsPending = false,
                    IsApproved = true,
                    IsRejected = false,
                    IsCompleted = false,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    EmployeeViewed = false,
                    CustomerViewed = true,
                    CustomerWasRecentResponse = true,
                    EmployeeWasRecentResponse = false
                };

                helpInfos2.Add(approvedHelpInfo);

                HelpInfo rejectedHelpInfo = new()
                {
                    Title = $"Help Title {i}",
                    Description = $"Help Description {i}",
                    Author = testAccountUserId2,
                    AuthorName = testAccountName2,
                    IsPending = false,
                    IsApproved = false,
                    IsRejected = true,
                    IsCompleted = false,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    EmployeeViewed = false,
                    CustomerViewed = true,
                    CustomerWasRecentResponse = true,
                    EmployeeWasRecentResponse = false
                };

                helpInfos2.Add(rejectedHelpInfo);

                HelpInfo completedHelpInfo = new()
                {
                    Title = $"Help Title {i}",
                    Description = $"Help Description {i}",
                    Author = testAccountUserId2,
                    AuthorName = testAccountName2,
                    IsPending = false,
                    IsApproved = false,
                    IsRejected = false,
                    IsCompleted = true,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    EmployeeViewed = false,
                    CustomerViewed = true,
                    CustomerWasRecentResponse = true,
                    EmployeeWasRecentResponse = false
                };

                helpInfos2.Add(completedHelpInfo);
            }

            context.Help.AddRange(helpInfos2);
            context.SaveChanges();

            return app;
        }
    }
}
