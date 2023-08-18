using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;

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
                List<IdentityRole> roles = new()
                {
                        new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                        new IdentityRole { Name = "Customer", NormalizedName = "USER" },
                        new IdentityRole { Name = "Employee", NormalizedName = "EMPLOYEE" },
                        new IdentityRole { Name = "Prospect", NormalizedName = "PROSPECT" }
                    };

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

            return app;
        }
    }
}
