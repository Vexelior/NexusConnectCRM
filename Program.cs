using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Extensions;
using NexusConnectCRM.Extensions.SignalR;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;

namespace NexusConnectCRM
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddMvc(options =>
                options.EnableEndpointRouting = false
            );

            builder.Services.AddSignalR();

            IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                                                  .Build();

            LoggingConfiguration nlogConfig = new NLogLoggingConfiguration(config.GetSection("NLog"));
            Logger logger = LogManager.Setup().LoadConfigurationFromSection(config.GetSection("NLog")).GetCurrentClassLogger();

            ColoredConsoleTarget consoleTarget = new("console")
            {
                Layout = $"[{DateTime.Now}]" + " ${level:uppercase=true} ${logger}.${callsite} [${callsite-linenumber}] - MESSAGE: ${message}${newline}${exception}"
            };
            nlogConfig.AddTarget(consoleTarget);
            nlogConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, consoleTarget);

            FileTarget fileTarget = new("fileTarget")
            {
                FileName = $"{Directory.GetCurrentDirectory()}/Logs/{DateTime.Now:yyyy-MM-dd}.log",
                Layout = $"[{DateTime.Now}]" + " (${level:uppercase=true}) - ${message}${newline}${exception}",
                KeepFileOpen = true,
                ArchiveFileName = $"{Directory.GetCurrentDirectory()}/Logs/Archive/NexusConnect.{{#}}.log",
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 2,
                ArchiveAboveSize = 102400
            };

            TraceTarget traceTarget = new("trace")
            {
                Layout = $"[{DateTime.Now}]" + " ${level:uppercase=true} ${logger}.${callsite} [${callsite-linenumber}] - MESSAGE: ${message}${newline}${exception}"
            };
            nlogConfig.AddTarget(traceTarget);
            nlogConfig.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, traceTarget);

            nlogConfig.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, consoleTarget, "Microsoft.AspNetCore.*", true);
            nlogConfig.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, traceTarget, "Microsoft.AspNetCore.*", true);

            nlogConfig.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, consoleTarget, "Microsoft.EntityFrameworkCore.*", true);
            nlogConfig.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Fatal, traceTarget, "Microsoft.EntityFrameworkCore.*", true);

            nlogConfig.AddTarget(fileTarget);
            nlogConfig.AddRuleForAllLevels(fileTarget, "NexusConnectCRM.*");

            LogManager.Configuration = nlogConfig;

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(nlogConfig);
            });

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;

                try
                {
                    ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

                    context.Database.Migrate();
                    app.SeedData();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while migrating or seeding the database.");
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseStatusCodePagesWithRedirects("/error/{0}");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/chat");
            app.MapHub<NotificationHub>("/notify");

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}