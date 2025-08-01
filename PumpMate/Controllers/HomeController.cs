using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PumpMate.Data;
using PumpMate.Models;

namespace PumpMate.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {


            int? userId = HttpContext.Session.GetInt32("UserId");
            string username = HttpContext.Session.GetString("Username");

            ViewBag.Username = username;

            if (userId == null)
            {
                // Kullanıcı giriş yapmamışsa, View'e boş model gönder
                return View(new List<Workout>());
            }

            // Kullanıcı giriş yapmışsa, workout'ları getir
            var workouts = _context.Workouts
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .Take(5)
                .ToList();

            // Kalori açığı hesaplama
            var today = DateTime.Today;
            var dailyCalories = _context.CalorieEntries
                .Where(c => c.UserId == userId && c.Date.Date == today)
                .Sum(c => c.Calories);

            var todayWorkouts = _context.Workouts
                .Where(w => w.UserId == userId && w.Date.Date == today)
                .Include(w => w.WorkoutDetails)
                .ToList();

            int burnedCalories = 0;
            foreach (var workout in todayWorkouts)
            {
                foreach (var detail in workout.WorkoutDetails)
                {
                    if (detail.Weight.HasValue && detail.Weight > 0)
                    {
                        burnedCalories += (int)((detail.Weight.Value * detail.Reps * 0.1) + (detail.Reps * 2)) * detail.SetCount;
                    }
                    else
                    {
                        burnedCalories += detail.Reps * 3 * detail.SetCount;
                    }
                }
            }

            ViewBag.DailyCalories = dailyCalories;
            ViewBag.BurnedCalories = burnedCalories;
            ViewBag.CalorieDeficit = dailyCalories - burnedCalories;

            return View(workouts);
        }
    }

}
