using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.ViewModels;
using System.Diagnostics;
using NLog;

namespace NexusConnectCRM.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        public async Task<IActionResult> Index()
        {
            _log.Info("Home page loaded.");
            return await Task.Run(() => View());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return await Task.Run(() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }
    }
}