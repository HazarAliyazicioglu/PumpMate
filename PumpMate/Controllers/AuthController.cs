using Microsoft.AspNetCore.Mvc;
using PumpMate.Data;
using PumpMate.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;

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
            try
            {
                if (ModelState.IsValid)
                {
                    // Kullanıcı adı kontrolü
                    if (_context.Users.Any(u => u.Username == user.Username))
                    {
                        ModelState.AddModelError("Username", "Bu kullanıcı adı zaten kullanılıyor.");
                        return View(user);
                    }

                    // Şifreyi hashle
                    user.Password = HashPassword(user.Password);
                    
                    _context.Users.Add(user);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Hesabınız başarıyla oluşturuldu. Giriş yapabilirsiniz.";
                    return RedirectToAction("Login");
                }
                return View(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.");
                return View(user);
            }
        }

        // GET: Auth/Login
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Kullanıcı adı ve şifre gereklidir.";
                    return View();
                }

                var hashedPassword = HashPassword(password);
                var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);

                if (user != null)
                {
                    HttpContext.Session.SetInt32("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Giriş sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotUsername()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotUsername(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    ViewBag.Error = "Şifre gereklidir.";
                    return View();
                }

                var hashedPassword = HashPassword(password);
                var user = _context.Users.FirstOrDefault(u => u.Password == hashedPassword);
                
                if (user != null)
                {
                    ViewBag.Username = user.Username;
                    ViewBag.SuccessMessage = "Kullanıcı adınız bulundu.";
                }
                else
                {
                    ViewBag.Error = "Bu şifreye ait kullanıcı bulunamadı.";
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "İşlem sırasında bir hata oluştu.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    ViewBag.Error = "Kullanıcı adı gereklidir.";
                    return View();
                }

                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    ViewBag.Password = "Şifreniz güvenlik nedeniyle gösterilemiyor. Lütfen yeni bir şifre oluşturun.";
                    ViewBag.SuccessMessage = "Kullanıcı bulundu.";
                }
                else
                {
                    ViewBag.Error = "Bu kullanıcı adına ait kayıt bulunamadı.";
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "İşlem sırasında bir hata oluştu.";
                return View();
            }
        }

        // Şifre hashleme metodu
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
