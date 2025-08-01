using Microsoft.AspNetCore.Mvc;
using PumpMate.Data;
using PumpMate.Models;

namespace PumpMate.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        private void SetUsernameToViewBag()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
        }

        public IActionResult Index()
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            // Kullanıcının istatistiklerini hesapla
            var workouts = _context.Workouts.Where(w => w.UserId == userId).ToList();
            
            ViewBag.TotalWorkouts = workouts.Count;
            ViewBag.ThisMonth = workouts.Count(w => w.Date >= DateTime.Today.AddDays(-30));
            ViewBag.ThisWeek = workouts.Count(w => w.Date >= DateTime.Today.AddDays(-7));
            
            // Günlük streak hesapla (son 7 günde kaç gün antrenman yapmış)
            var last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Today.AddDays(-i)).ToList();
            ViewBag.Streak = last7Days.Count(date => workouts.Any(w => w.Date.Date == date));

            return View(user);
        }

        [HttpPost]
        public IActionResult Update(User updatedUser, string newPassword)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                user.Username = updatedUser.Username;
                HttpContext.Session.SetString("Username", user.Username); // BURADA GÜNCELLENDİ

                if (!string.IsNullOrEmpty(newPassword))
                {
                    user.Password = newPassword;
                }

                _context.SaveChanges();
                ViewBag.Message = "Profil başarıyla güncellendi.";
            }

            return View("Index", user);
        }

    }
}
