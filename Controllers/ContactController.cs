using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusConnectCRM.Data;
using NexusConnectCRM.Data.Models.Identity;
using NexusConnectCRM.Data.Models.Newsletter;
using NexusConnectCRM.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace NexusConnectCRM.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILogger<ContactController> _logger;

        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitContact(ContactViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            if (ModelState.IsValid)
            {
                MailMessage mailMessage = new()
                {
                    From = new MailAddress("nexusconnectcrm@gmail.com", "NexusConnect")
                };
                mailMessage.To.Add("nexusconnectcrm@gmail.com");
                mailMessage.Subject = "Contact Form Message";
                StringBuilder stringBuilder = new();
                stringBuilder.Append($"Name: {viewModel.Name}<br/>Email: {viewModel.Email}<br/>Subject: {viewModel.Subject}<br/>Message: {viewModel.Message}");
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = stringBuilder.ToString();

                using SmtpClient smtpClient = new("smtp.gmail.com", 587);
                smtpClient.Credentials = new NetworkCredential("nexusconnectcrm@gmail.com", "qbur pikt hrqz wsuz");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

                TempData["StatusMessage"] = "Thank you for your message! We will get back to you as soon as possible.";
            }
            else
            {
                TempData["StatusMessage"] = "There was an error submitting your message. Please try again.";

                return View("Index", viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> NewsletterSignUp(NewsletterViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            if (ModelState.IsValid)
            {
                ApplicationDbContext dbContext = new();

                if (dbContext.Users.Any(n => n.Email == viewModel.Email))
                {
                    ApplicationUser user = await dbContext.Users.FirstOrDefaultAsync(n => n.Email == viewModel.Email);
                    Newsletter newsletter = await dbContext.Newsletters.FirstOrDefaultAsync(n => n.Email == user.Email);

                    if (newsletter is not null && dbContext.Newsletters.Any(n => n.Email == user.Email))
                    {
                        newsletter.IsSubscribed = true;
                    }
                    else
                    {
                        Newsletter newNewsletter = new()
                        {
                            Email = user.Email,
                            IsSubscribed = true,
                            UserId = user.Id
                        };

                        await dbContext.Newsletters.AddAsync(newNewsletter);
                    }
                }
                else
                {
                    Newsletter newsletter = new()
                    {
                        Email = viewModel.Email,
                        IsSubscribed = true,
                    };

                    await dbContext.Newsletters.AddAsync(newsletter);
                }

                try
                {
                    TempData["StatusMessage"] = "Thank you for signing up for our newsletter!";
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    TempData["StatusMessage"] = "There was an error signing up for our newsletter. Please try again.";
                    return RedirectToAction("Index", "Home", new { emailSignup = false });
                }

                MailMessage mailMessage = new()
                {
                    From = new MailAddress("nexusconnectcrm@gmail.com", "NexusConnect")
                };
                mailMessage.To.Add(viewModel.Email);
                mailMessage.Subject = "Newsletter Sign Up";
                StringBuilder stringBuilder = new();
                stringBuilder.Append($"Thank you for signing up for our newsletter!<br/><br/>As a reminder, you signed up with the following email address: {viewModel.Email}");
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = stringBuilder.ToString();

                using SmtpClient smtpClient = new("smtp.gmail.com", 587);
                smtpClient.Credentials = new NetworkCredential("nexusconnectcrm@gmail.com", "qbur pikt hrqz wsuz");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Index", viewModel);
            }
        }
    }
}
