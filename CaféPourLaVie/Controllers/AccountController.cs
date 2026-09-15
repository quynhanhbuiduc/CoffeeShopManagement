using CaféPourLaVie.Data;
using CaféPourLaVie.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CaféPourLaVie.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IPasswordHasher<Account> _passwordHasher;

        public AccountController(ApplicationDbContext context, IPasswordHasher<Account> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }


        // =========================
        // LOGIN
        // =========================
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
                .FirstOrDefaultAsync(a => a.Username == username);

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

            var result = _passwordHasher.VerifyHashedPassword(
                account,
                account.Password,
                password
            );

            if (result == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim("AccountId", account.AccountId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

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


        // =========================
        // LOGOUT
        // =========================
        // GET Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");

            return RedirectToAction("Login");
        }


        //=========================
        // ACCESS DENIED    
        // =========================
        // GET Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}