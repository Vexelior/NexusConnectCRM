using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data;
using System.Data;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Areas.Employee.ViewModels;
using NexusConnectCRM.Data.Models.Help;

namespace NexusConnectCRM.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee, Admin")]
    public class HelpController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HelpController(ApplicationDbContext context,
                                  UserManager<ApplicationUser> userManager,
                                  IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();

                string[] splitString = searchQuery.Split(" ").Where(x => x != "").ToArray();
                string firstWord = splitString[0];

                if (splitString.Length > 1)
                {
                    string secondWord = splitString[1];

                    var newQuery = await _context.Help.Where(h => h.Title.ToLower().Contains(firstWord) &&
                                                                           h.Title.ToLower().Contains(secondWord))
                                                      .OrderByDescending(h => h.CreatedDate)
                                                      .ToListAsync();

                    ListHelpViewModel searchModel = new()
                    {
                        HelpList = newQuery,
                        PageNumber = 1,
                        PageSize = 10,
                        TotalPages = 1
                    };

                    return View("Help", searchModel);
                }
                else
                {
                    var newQuery = await _context.Help.Where(h => h.Title.ToLower().Contains(firstWord))
                                                      .OrderByDescending(h => h.CreatedDate)
                                                      .ToListAsync();

                    ListHelpViewModel searchModel = new()
                    {
                        HelpList = newQuery,
                        PageNumber = 1,
                        PageSize = 10,
                        TotalPages = 1
                    };

                    return View("Help", searchModel);
                }
            }

            int pageNumber = 1;
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((decimal)_context.Help.Count() / pageSize);

            ListHelpViewModel viewModel = new()
            {
                HelpList = await _context.Help.OrderByDescending(x => x.CreatedDate)
                                               .Skip((pageNumber - 1) * 10)
                                               .Take(pageSize)
                                               .ToListAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View("Help", viewModel);
        }

        public async Task<IActionResult> ShowTickets(int pageNumber)
        {
            int pageSize = 10;
            int totalPages = (int)Math.Ceiling((decimal)_context.Help.Count() / 10);

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            else if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            ListHelpViewModel viewModel = new()
            {
                HelpList = await _context.Help.OrderByDescending(x => x.CreatedDate)
                                               .Skip((pageNumber - 1) * 10)
                                               .Take(pageSize)
                                               .ToListAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View("Help", viewModel);
        }

        public async Task<IActionResult> HelpEdit(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
            {
                return NotFound();
            }

            List<HelpResponseInfo> feedback = await _context.HelpFeedback.Where(h => h.ResponseId == help.Id).ToListAsync();

            HelpEditViewModel viewModel = new()
            {
                Id = id,
                Help = help,
                HelpResponses = feedback
            };

            await SetEmployeeViewed(id);

            return View("HelpEdit", viewModel);
        }

        public async Task SetEmployeeViewed(int id)
        {
            HelpInfo help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
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

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });

        }

        public async Task<IActionResult> HelpApprove(int id)
        {
            var help = await _context.Help.FirstOrDefaultAsync(m => m.Id == id);

            if (help == null)
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

            if (help == null)
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

            if (help == null)
            {
                return NotFound();
            }

            help.IsCompleted = true;

            _context.Update(help);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HelpEdit), new { id = help.Id, author = help.Author });
        }

        public async Task<IActionResult> NewHelp()
        {
            var helpList = await _context.Help.Where(h => h.CustomerWasRecentResponse &&
                                                                   h.IsApproved &&
                                                                  !h.IsCompleted)
                                              .OrderByDescending(h => h.CreatedDate)
                                              .ToListAsync();

            ListHelpViewModel viewModel = new()
            {
                HelpList = helpList,
            };

            return View("NewHelp", viewModel);
        }

        public async Task<IActionResult> PendingHelp()
        {
            var helpList = await _context.Help.Where(h => h.IsPending)
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
            var helpList = await _context.Help.Where(h => h.IsRejected)
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
