using Microsoft.AspNetCore.Mvc;
using NexusConnectCRM.ViewModels;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;

namespace NexusConnectCRM.Controllers
{
    public class ContactController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitContact(ContactViewModel viewModel)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            if (ModelState.IsValid)
            {
                MimeMessage mailMessage = new();

                mailMessage.To.Add(new MailboxAddress("NexusConnect", "nexusconnectcrm@gmail.com"));
                mailMessage.From.Add(new MailboxAddress("NexusConnect", "nexusconnect@gmail.com"));
                mailMessage.Subject = "Contact Form Message";
                StringBuilder stringBuilder = new();
                stringBuilder.Append($"Name: {viewModel.Name}<br/>Email: {viewModel.Email}<br/>Subject: {viewModel.Subject}<br/>Message: {viewModel.Message}<br/><br/>This email was sent from the Contact page.");
                mailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = stringBuilder.ToString()
                };

                using SmtpClient smtpClient = new();
                smtpClient.Connect("smtp.gmail.com", 587, false);
                smtpClient.Authenticate("nexusconnectcrm@gmail.com", "qbur pikt hrqz wsuz");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
            else
            {
                return View("Index", viewModel);
            }

            // Redirect to the Home page.
            return RedirectToAction("Index", "Home");
        }
    }
}
