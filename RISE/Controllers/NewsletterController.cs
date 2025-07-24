using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Models;

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
            if(string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            if(!_context.NewsletterSubscribers.Any(e => e.Email == email))
            {
                _context.NewsletterSubscribers.Add(new NewsletterSubscriber { Email = email });
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home", new { subscribed = true });
        }
    }
}