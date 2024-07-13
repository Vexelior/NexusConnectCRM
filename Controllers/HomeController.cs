using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.ViewModels;
using System.Diagnostics;

namespace NexusConnectCRM.Controllers
{
    public class HomeController(ILogger<HomeController> _logger) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}