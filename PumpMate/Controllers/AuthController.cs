using Microsoft.AspNetCore.Mvc;
using PumpMate.Data;
using PumpMate.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PumpMate.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Auth/Register
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {

            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(user);
        }


        // GET: Auth/Login
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Index", "Home"); // Burayı değiştirdik
            }

            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); // Ana sayfaya yönlendiriyoruz
        }

        [HttpGet]
        public IActionResult ForgotUsername()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotUsername(string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Password == password);
            if (user != null)
            {
                ViewBag.Username = user.Username;
            }
            else
            {
                ViewBag.Error = "Şifreye ait kullanıcı bulunamadı.";
            }

            return View();
        }

        // Şifre unuttum sayfası
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                ViewBag.Password = user.Password;
            }
            else
            {
                ViewBag.Error = "Kullanıcı adına ait kayıt bulunamadı.";
            }

            return View();
        }
    }
}
