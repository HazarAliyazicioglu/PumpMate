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

        public IActionResult Index()
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var today = DateTime.Today;
            var weekStart = today.AddDays(-(int)today.DayOfWeek);

            var viewModel = new CalorieViewModel();

            // Günlük kalori girişleri
            var dailyEntries = _context.CalorieEntries
                .Where(c => c.UserId == userId && c.Date.Date == today)
                .OrderByDescending(c => c.Date)
                .ToList();

            viewModel.CalorieEntries = dailyEntries;
            viewModel.TotalCaloriesConsumed = dailyEntries.Sum(c => c.Calories);

            // Haftalık kalori girişleri
            var weeklyEntries = _context.CalorieEntries
                .Where(c => c.UserId == userId && c.Date >= weekStart)
                .ToList();

            viewModel.WeeklyCaloriesConsumed = weeklyEntries.Sum(c => c.Calories);

            // Antrenman kalori hesaplama
            var workouts = _context.Workouts
                .Where(w => w.UserId == userId)
                .Include(w => w.WorkoutDetails)
                .ThenInclude(wd => wd.Exercise)
                .OrderByDescending(w => w.Date)
                .ToList();

            foreach (var workout in workouts)
            {
                var workoutCalorieInfo = new WorkoutCalorieInfo
                {
                    Date = workout.Date,
                    WorkoutType = workout.WorkoutType,
                    Exercises = new List<ExerciseCalorieInfo>()
                };

                int totalWorkoutCalories = 0;

                foreach (var detail in workout.WorkoutDetails)
                {
                    int caloriesBurned = CalculateExerciseCalories(detail);
                    totalWorkoutCalories += caloriesBurned;

                    workoutCalorieInfo.Exercises.Add(new ExerciseCalorieInfo
                    {
                        ExerciseName = detail.Exercise.Name,
                        SetCount = detail.SetCount,
                        Reps = detail.Reps,
                        Weight = detail.Weight,
                        CaloriesBurned = caloriesBurned
                    });
                }

                workoutCalorieInfo.TotalCaloriesBurned = totalWorkoutCalories;
                viewModel.WorkoutCalories.Add(workoutCalorieInfo);
            }

            // Günlük ve haftalık kalori yakma
            var todayWorkouts = viewModel.WorkoutCalories.Where(w => w.Date.Date == today).ToList();
            var weeklyWorkouts = viewModel.WorkoutCalories.Where(w => w.Date >= weekStart).ToList();

            viewModel.TotalCaloriesBurned = todayWorkouts.Sum(w => w.TotalCaloriesBurned);
            viewModel.WeeklyCaloriesBurned = weeklyWorkouts.Sum(w => w.TotalCaloriesBurned);

            // Kalori açığı hesaplama
            viewModel.CalorieDeficit = viewModel.TotalCaloriesConsumed - viewModel.TotalCaloriesBurned;
            viewModel.WeeklyCalorieDeficit = viewModel.WeeklyCaloriesConsumed - viewModel.WeeklyCaloriesBurned;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddEntry(CalorieEntry entry)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            // Debug için ModelState hatalarını kontrol et
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Message"] = $"Validation hatası: {string.Join(", ", errors)}";
                return RedirectToAction("Index");
            }

            try
            {
                entry.UserId = userId.Value;
                entry.Date = DateTime.Today;
                
                _context.CalorieEntries.Add(entry);
                _context.SaveChanges();

                TempData["Message"] = "Kalori girişi başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                TempData["Message"] = $"Hata oluştu: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteEntry(int id)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var entry = _context.CalorieEntries.FirstOrDefault(c => c.Id == id && c.UserId == userId);
            if (entry != null)
            {
                _context.CalorieEntries.Remove(entry);
                _context.SaveChanges();
                TempData["Message"] = "Kalori girişi silindi.";
            }

            return RedirectToAction("Index");
        }

        private int CalculateExerciseCalories(WorkoutDetail detail)
        {
            // Basit kalori hesaplama formülü
            // Her set için: (Ağırlık * Tekrar * 0.1) + (Tekrar * 2)
            // Eğer ağırlık yoksa: Tekrar * 3
            
            int caloriesPerSet = 0;
            
            if (detail.Weight.HasValue && detail.Weight > 0)
            {
                // Ağırlıklı egzersiz
                caloriesPerSet = (int)((detail.Weight.Value * detail.Reps * 0.1) + (detail.Reps * 2));
            }
            else
            {
                // Ağırlıksız egzersiz
                caloriesPerSet = detail.Reps * 3;
            }

            return caloriesPerSet * detail.SetCount;
        }
    }
} 