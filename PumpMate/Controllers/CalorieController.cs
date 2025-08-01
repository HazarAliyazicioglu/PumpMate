using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PumpMate.Data;
using PumpMate.Models;

namespace PumpMate.Controllers
{
    public class CalorieController : Controller
    {
        private readonly AppDbContext _context;

        public CalorieController(AppDbContext context)
        {
            _context = context;
        }

        private void SetUsernameToViewBag()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
        }

        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        public IActionResult Index()
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                var today = DateTime.Today;
                var weekStart = today.AddDays(-(int)today.DayOfWeek);

                var viewModel = new CalorieViewModel();

                // Günlük kalori girişleri
                var dailyEntries = _context.CalorieEntries
                    .Where(c => c.UserId == userId && c.Date == today)
                    .OrderByDescending(c => c.Date)
                    .ToList();
                
                viewModel.CalorieEntries = dailyEntries;
                viewModel.TotalCaloriesConsumed = dailyEntries.Sum(c => c.Calories);
                viewModel.TotalCaloriesBurned = CalculateDailyBurnedCalories(userId.Value, today);

                // Haftalık kalori girişleri
                var weeklyEntries = _context.CalorieEntries
                    .Where(c => c.UserId == userId && c.Date >= weekStart && c.Date <= today)
                    .ToList();
                
                viewModel.WeeklyCaloriesConsumed = weeklyEntries.Sum(c => c.Calories);
                viewModel.WeeklyCaloriesBurned = CalculateWeeklyBurnedCalories(userId.Value, weekStart, today);

                // Kalori açığı hesaplama
                viewModel.CalorieDeficit = viewModel.TotalCaloriesConsumed - viewModel.TotalCaloriesBurned;
                viewModel.WeeklyCalorieDeficit = viewModel.WeeklyCaloriesConsumed - viewModel.WeeklyCaloriesBurned;
                
                // Günlük hedef (varsayılan 2000 kalori)
                viewModel.DailyGoal = 2000;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Kalori takibi yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        private int CalculateDailyBurnedCalories(int userId, DateTime date)
        {
            try
            {
                var workouts = _context.Workouts
                    .Include(w => w.WorkoutDetails)
                    .ThenInclude(wd => wd.Exercise)
                    .Where(w => w.UserId == userId && w.Date == date)
                    .ToList();

                int totalBurned = 0;

                foreach (var workout in workouts)
                {
                    totalBurned += CalculateWorkoutCalories(workout);
                }

                return totalBurned;
            }
            catch
            {
                return 0;
            }
        }

        private int CalculateWeeklyBurnedCalories(int userId, DateTime weekStart, DateTime weekEnd)
        {
            try
            {
                var workouts = _context.Workouts
                    .Include(w => w.WorkoutDetails)
                    .ThenInclude(wd => wd.Exercise)
                    .Where(w => w.UserId == userId && w.Date >= weekStart && w.Date <= weekEnd)
                    .ToList();

                int totalBurned = 0;

                foreach (var workout in workouts)
                {
                    totalBurned += CalculateWorkoutCalories(workout);
                }

                return totalBurned;
            }
            catch
            {
                return 0;
            }
        }

        private int CalculateWorkoutCalories(Workout workout)
        {
            int totalCalories = 0;

            // Antrenman türüne göre baz kalori
            int baseCalories = workout.WorkoutType switch
            {
                "Gym" => 300,      // Ağırlık antrenmanı
                "Home" => 200,     // Ev antrenmanı
                "Cardio" => 400,   // Kardiyo
                "Yoga" => 150,     // Yoga
                _ => 250
            };

            // Egzersiz detaylarına göre ek kalori
            foreach (var detail in workout.WorkoutDetails)
            {
                int exerciseCalories = CalculateExerciseCalories(detail);
                totalCalories += exerciseCalories;
            }

            // Eğer hiç egzersiz detayı yoksa baz kaloriyi döndür
            if (!workout.WorkoutDetails.Any())
            {
                return baseCalories;
            }

            return totalCalories;
        }

        private int CalculateExerciseCalories(WorkoutDetail detail)
        {
            // Egzersiz başına yakılan kalori hesaplama
            // Bu basit bir hesaplama, daha gelişmiş algoritmalar kullanılabilir
            
            int baseCalories = 10; // Her set için baz kalori
            int weightMultiplier = detail.Weight.HasValue ? (int)(detail.Weight.Value / 10) : 1;
            int repMultiplier = detail.Reps / 10;
            
            int caloriesPerSet = baseCalories + weightMultiplier + repMultiplier;
            
            return caloriesPerSet * detail.SetCount;
        }

        [HttpPost]
        public IActionResult AddCalorieEntry(CalorieEntry entry)
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                // Validation kontrolleri
                if (string.IsNullOrEmpty(entry.FoodName))
                {
                    TempData["ErrorMessage"] = "Yiyecek/İçecek adı gereklidir.";
                    return RedirectToAction("Index");
                }

                if (entry.Calories <= 0)
                {
                    TempData["ErrorMessage"] = "Kalori değeri 0'dan büyük olmalıdır.";
                    return RedirectToAction("Index");
                }
                
                entry.UserId = userId.Value;
                entry.Date = DateTime.Today;

                _context.CalorieEntries.Add(entry);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Kalori girişi başarıyla eklendi!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kalori girişi eklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult AddEntry(CalorieEntry entry)
        {
            return AddCalorieEntry(entry);
        }

        [HttpPost]
        public IActionResult DeleteCalorieEntry(int id)
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                var entry = _context.CalorieEntries.FirstOrDefault(c => c.Id == id && c.UserId == userId);
                if (entry == null)
                {
                    TempData["ErrorMessage"] = "Kalori girişi bulunamadı.";
                    return RedirectToAction("Index");
                }

                _context.CalorieEntries.Remove(entry);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Kalori girişi başarıyla silindi!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kalori girişi silinirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DeleteEntry(int id)
        {
            return DeleteCalorieEntry(id);
        }
    }
} 