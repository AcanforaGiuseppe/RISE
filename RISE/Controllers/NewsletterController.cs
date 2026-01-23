/* RISE PROJECT - 2026 - COPYRIGHT by Acanfora Giuseppe */
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;
using System.Net.Mail;

namespace RISE.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsletterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Subscribe(string email)
        {
            if(!string.IsNullOrEmpty(email) && !_context.NewsletterSubscribers.Any(x => x.Email == email))
            {
                _context.NewsletterSubscribers.Add(new NewsletterSubscriber { Email = email });
                _context.SaveChanges();

                try
                {
                    using var smtp = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new System.Net.NetworkCredential("TUA_EMAIL@gmail.com", "TUA_PASSWORD_APP"),
                        EnableSsl = true
                    };
                    smtp.Send("TUA_EMAIL@gmail.com", email, "Welcome to RISE Newsletter", "Thanks for subscribing to RISE Calisthenics!");
                }
                catch { }
            }

            return RedirectToAction("Index", "Home");
        }

    }
}