using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data;
using System.Data;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Employee.ViewModels;
using NexusConnectCRM.Data.Models.Help;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NexusConnectCRM.Extensions.SignalR;

namespace NexusConnectCRM.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee,Admin,HeadAdmin,Help Desk")]
    public class HelpController(ApplicationDbContext context,
                          UserManager<ApplicationUser> userManager,
                          IWebHostEnvironment hostEnvironment,
                          IHubContext<NotificationHub> hubContext) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IWebHostEnvironment _hostEnvironment = hostEnvironment;
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;

        public async Task<IActionResult> Index(string searchQuery, int page = 1, int pageSize = 10)
        {
            List<HelpInfo> help = [];

            searchQuery ??= "";

            IQueryable<HelpInfo> query = _context.Help;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(x => x.Title.Contains(searchQuery));
            }

            help = await query.OrderByDescending(x => x.CreatedDate)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

            if (help is null)
            {
                return NotFound();
            }

            int totalHelp = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalHelp / pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            ListHelpViewModel viewModel = new()
            {
                HelpList = help,
                SearchQuery = searchQuery,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalHelp = totalHelp
            };

            return View("Help", viewModel);
        }

        public async Task<IActionResult> HelpEdit(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help is null)
            {
                return NotFound();
            }

            var feedback = await _context.HelpFeedback.Where(h => h.ResponseId == help.Id).ToListAsync();

            HelpEditViewModel viewModel = new()
            {
                Id = id,
                Help = help,
                HelpResponses = feedback,
                Responder = await _userManager.GetUserIdAsync(await _userManager.GetUserAsync(User))
            };

            await SetEmployeeViewed(id);

            return View("HelpEdit", viewModel);
        }

        public async Task SetEmployeeViewed(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help is null)
            {
                return;
            }
            try
            {
                help.EmployeeViewed = true;
                _context.Update(help);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelpExists(help.Id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitHelpResponse(int id, HelpEditViewModel viewModel)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help is null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string name = _context.Users.Where(u => u.Id == _userManager.GetUserId(User)).Select(u => u.FirstName).FirstOrDefault();

                DateTime date = DateTime.Now;
                string dateString = date.ToString("MM/dd/yyyy h:mmtt");

                string message = $"[{dateString}] {name}: \n {viewModel.Response}";

                HelpResponseInfo feedback = new()
                {
                    Response = message,
                    Author = _userManager.GetUserId(User),
                    IsEmployee = true,
                    CreatedDate = help.CreatedDate,
                    ModifiedDate = DateTime.Now,
                    Image = viewModel.Image,
                    ResponseId = help.Id
                };

                help.ModifiedDate = feedback.ModifiedDate;
                help.CustomerWasRecentResponse = false;
                help.EmployeeWasRecentResponse = true;

                if (viewModel.Image != null)
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
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> HelpApprove(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help is null)
            {
                return NotFound();
            }

            help.IsApproved = true;
            help.IsPending = false;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> HelpReject(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help is null)
            {
                return NotFound();
            }

            help.IsRejected = true;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> HelpCompleted(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help is null)
            {
                return NotFound();
            }

            help.IsCompleted = true;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> NewHelp(string searchQuery, int page = 1, int pageSize = 10)
        {
            List<HelpInfo> newHelp = [];

            searchQuery ??= "";

            IQueryable<HelpInfo> query = _context.Help.Where(h => h.IsPending && !h.IsRejected);

            query = query.Where(x => x.Title.Contains(searchQuery) ||
                                              x.AuthorName.Contains(searchQuery));

            newHelp = await query.OrderByDescending(x => x.CreatedDate)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();

            if (newHelp is null)
            {
                return NotFound();
            }

            int totalHelp = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalHelp / pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }
            
            ListHelpViewModel viewModel = new()
            {
                HelpList = newHelp,
                SearchQuery = searchQuery,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalHelp = totalHelp
            };

            return View("NewHelp", viewModel);
        }

        public async Task<IActionResult> PendingHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsPending && !h.IsRejected)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("PendingHelp", viewModel);
        }

        public async Task<IActionResult> RejectedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsRejected && !h.IsApproved)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("RejectedHelp", viewModel);
        }

        public async Task<IActionResult> CompletedHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsCompleted)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("CompletedHelp", viewModel);
        }

        private bool HelpExists(int id)
        {
            return _context.Help.Any(e => e.Id == id);
        }
    }
}
