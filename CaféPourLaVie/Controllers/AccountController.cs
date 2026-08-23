using CaféPourLaVie.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaféPourLaVie.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET Account/Login
        public IActionResult Login()
        {
            return View();
        }
        // POST Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a =>
                    a.Username == username &&
                    a.Password == password);


            if (account == null)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }


            if (!account.Status)
            {
                ViewBag.Error = "Tài khoản đã bị khóa";
                return View();
            }


            // Create Claims
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.Name,
                    account.Username),

                new Claim(
                    ClaimTypes.Role,
                    account.Role),

                new Claim(
                    "AccountId",
                    account.AccountId.ToString())
            };


            var identity = new ClaimsIdentity(
                claims,
                "CookieAuth");


            var principal = new ClaimsPrincipal(identity);


            await HttpContext.SignInAsync(
                "CookieAuth",
                principal);


            if (account.Role == "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            if (account.Role == "Employee")
            {
                return RedirectToAction("Index", "Sales");
            }

            return RedirectToAction("Login");
        }

        // GET Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");

            return RedirectToAction("Login");
        }

        // GET Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
