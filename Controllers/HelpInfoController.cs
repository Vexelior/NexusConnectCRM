using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Help;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.ViewModels.HelpInfoData;

namespace NexusConnectCRM.Controllers
{
    [Authorize]
    public class HelpInfoController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> userManager,
                                    IWebHostEnvironment hostEnvironment) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            int newResponses = await _context.Help.Where(h => h.Author == user.Id &&
                                                                       h.IsApproved &&
                                                                       h.EmployeeWasRecentResponse)
                                                  .CountAsync();

            HelpInfoIndexViewModel viewModel = new()
            {
                NewResponses = newResponses
            };

            if (user is null)
            {
                return NotFound();
            }

            return View("Index", viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResponseId,Title,Description,Image,Author,IsPending,IsApproved,IsRejected,IsCompleted")] HelpInfo helpInfo)
        {
            string authorId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                helpInfo.Author = authorId;
                helpInfo.IsPending = true;
                helpInfo.IsApproved = false;
                helpInfo.IsCompleted = false;
                helpInfo.IsRejected = false;
                helpInfo.CreatedDate = DateTime.Now;
                helpInfo.ModifiedDate = DateTime.Now;
                helpInfo.CustomerViewed = true;
                helpInfo.EmployeeViewed = false;
                helpInfo.CustomerWasRecentResponse = true;
                helpInfo.EmployeeWasRecentResponse = false;

                if (helpInfo.Image is not null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(helpInfo.Image.FileName);
                    string extension = Path.GetExtension(helpInfo.Image.FileName);
                    helpInfo.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/help/", fileName);

                    if (!Directory.Exists(wwwRootPath + "/images/help/"))
                    {
                        Directory.CreateDirectory(wwwRootPath + "/images/help/");
                    }

                    using var fileStream = new FileStream(path, FileMode.Create);
                    await helpInfo.Image.CopyToAsync(fileStream);
                }

                _context.Add(helpInfo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(helpInfo);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                return NotFound();
            }

            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help.Author != user.Id)
            {
                return NotFound();
            }

            var fullName = _context.Users.Where(u => u.Id == user.Id)
                                         .Select(u => u.FirstName + " " + u.LastName)
                                         .FirstOrDefault();

            var feedback = await _context.HelpFeedback.Where(h => h.ResponseId == help.Id).ToListAsync();

            HelpInfoEditViewModel viewModel = new()
            {
                Id = id,
                FullName = fullName,
                Help = help,
                HelpResponses = feedback
            };

            await SetCustomerViewed(id);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitHelpResponse(int id, HelpInfoEditViewModel viewModel)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            string name = _context.Users.Where(u => u.Id == _userManager.GetUserId(User))
                                        .Select(u => u.FirstName + " " + u.LastName)
                                        .FirstOrDefault();

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
                Image = viewModel.Image,
                ResponseId = help.Id
            };

            help.IsCompleted = false;
            help.ModifiedDate = feedback.ModifiedDate;
            help.CustomerWasRecentResponse = true;
            help.EmployeeWasRecentResponse = false;
            help.EmployeeViewed = false;

            if (viewModel.Image is not null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(feedback.Image.FileName);
                string extension = Path.GetExtension(feedback.Image.FileName);
                feedback.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/help/responses/", fileName);

                if (!Directory.Exists(wwwRootPath + "/images/help/responses/"))
                {
                    Directory.CreateDirectory(wwwRootPath + "/images/help/responses/");
                }

                using var fileStream = new FileStream(path, FileMode.Create);
                await feedback.Image.CopyToAsync(fileStream);
            }

            _context.Add(feedback);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = help.Id });
        }

        public async Task SetCustomerViewed(int id)
        {
            var helpTicket = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            try
            {
                helpTicket.CustomerViewed = true;
                _context.Update(helpTicket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelpInfoExists(helpTicket.Id))
                {
                    throw new Exception("Help ticket not found.");
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOpenTickets()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var openTickets = await _context.Help.Where(u => u.Author == user.Id)
                                                 .OrderByDescending(u => u.CreatedDate)
                                                 .ToListAsync();

            return PartialView("_Tickets", openTickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetRejectedTickets()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var rejectedTickets = await _context.Help.Where(u => u.Author == user.Id &&
                                                                          u.IsRejected)
                                                     .OrderByDescending(u => u.CreatedDate)
                                                     .ToListAsync();


            return PartialView("_Tickets", rejectedTickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingTickets()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var pendingTickets = await _context.Help.Where(u => u.Author == user.Id &&
                                                                         u.IsPending && u.CustomerWasRecentResponse &&
                                                                         !u.IsRejected &&
                                                                         !u.IsCompleted &&
                                                                         !u.IsApproved)
                                                    .OrderByDescending(u => u.CreatedDate)
                                                    .ToListAsync();

            return PartialView("_Tickets", pendingTickets);
        }

        [HttpGet]
        public async Task<IActionResult> GetRespondedTickets()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var respondedTickets = await _context.Help.Where(u => u.Author == user.Id &&
                                                                         u.IsApproved && u.EmployeeWasRecentResponse)
                                                      .OrderByDescending(u => u.CreatedDate)
                                                      .ToListAsync();


            return PartialView("_Tickets", respondedTickets);
        }


        [HttpGet]
        public async Task<IActionResult> GetClosedTickets()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var closedTickets = await _context.Help.Where(u => u.Author == user.Id &&
                                                                        u.IsCompleted)
                                                   .OrderByDescending(u => u.CreatedDate)
                                                   .ToListAsync();


            return PartialView("_Tickets", closedTickets);
        }

        private bool HelpInfoExists(int id)
        {
            return _context.Help.Any(e => e.Id == id);
        }
    }
}
