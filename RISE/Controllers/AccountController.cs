using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RISE.Data;
using RISE.Security;
using System.Security.Claims;

namespace RISE.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ================= ADMIN LOGIN =================
        [HttpGet]
        public IActionResult AdminLogin() => View();

        [HttpPost]
        public async Task<IActionResult> AdminLogin(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == username && u.Role == "Admin");

            if(user == null || !PasswordHasher.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, "AdminCookie");
            await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(identity));

            return Redirect("/Admin/Dashboard");
        }

        // ================= USER LOGIN =================
        [HttpGet]
        public IActionResult UserLogin() => View();

        [HttpPost]
        public async Task<IActionResult> UserLogin(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == username && u.Role == "User");

            if(user == null || !PasswordHasher.Verify(password, user.PasswordHash))
            {
                ViewBag.Error = "Invalid credentials";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var identity = new ClaimsIdentity(claims, "UserCookie");
            await HttpContext.SignInAsync("UserCookie", new ClaimsPrincipal(identity));

            return Redirect("/");
        }

        // ================= LOGOUT =================
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminCookie");
            await HttpContext.SignOutAsync("UserCookie");
            return Redirect("/");
        }
    }
}
