using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Employee.ViewModels;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Help;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.ViewModels;

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

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help.Author != user.Id)
            {
                return NotFound();
            }

            List<HelpResponseInfo> feedback = await _context.HelpFeedback.Where(h => h.ResponseId == help.Id).ToListAsync();

            AuthorHelpEditViewModel viewModel = new()
            {
                Id = id,
                Help = help,
                HelpResponses = feedback
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitHelpResponse(int id, HelpEditViewModel viewModel)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            string name = _context.Users.Where(u => u.Id == _userManager.GetUserId(User)).Select(u => u.FirstName + " " + u.LastName).FirstOrDefault();

            DateTime date = DateTime.Now;
            string dateString = date.ToString("MM/dd/yyyy h:mmtt");

            string message = $"[{dateString}] {name}: \n {viewModel.Response}";

            HelpResponseInfo feedback = new()
            {
                Response = message,
                Author = _userManager.GetUserId(User),
                IsEmployee = false,
                CreatedDate = help.CreatedDate,
                ModifiedDate = DateTime.Now,
                Image = null,
                ResponseId = help.Id
            };

            help.IsPending = true;
            help.IsCompleted = false;
            help.ModifiedDate = feedback.ModifiedDate;

            _context.Add(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = help.Id });
        }


        private bool HelpInfoExists(int id)
        {
            return _context.Help.Any(e => e.Id == id);
        }
    }
}
