using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Help;
using NexusConnectCRM.Data.Models.Identity;

namespace NexusConnectCRM.Controllers
{
    public class HelpInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HelpInfoController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HelpInfo/Details/5
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var userHelpTickets = await _context.Help.Where(u => u.Author == user.Id).ToListAsync();

            return userHelpTickets == null ? throw new Exception("No help tickets found for this user.") : (IActionResult)View(userHelpTickets);
        }

        // GET: HelpInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResponseId,Title,Description,Image,Author,IsPending,IsApproved,IsRejected,IsCompleted")] HelpInfo helpInfo)
        {
            string authorId = _userManager.GetUserId(User);

            helpInfo.Author = authorId;
            helpInfo.IsPending = true;
            helpInfo.IsApproved = false;
            helpInfo.IsCompleted = false;
            helpInfo.IsRejected = false;
            helpInfo.ResponseId = authorId;
            helpInfo.CreatedDate = DateTime.Now;
            helpInfo.ModifiedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(helpInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(helpInfo);
        }

        // Edit : HelpInfo/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var helpInfo = await _context.Help.FindAsync(id);

            if (helpInfo.Author != user.Id)
            {
                return NotFound();
            }

            return View(helpInfo);
        }

       
        private bool HelpInfoExists(int id)
        {
            return _context.Help.Any(e => e.Id == id);
        }
    }
}
