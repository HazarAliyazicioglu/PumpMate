using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PumpMate.Data;
using PumpMate.Models;

namespace PumpMate.Controllers
{
    public class WorkoutController : Controller
    {
        private readonly AppDbContext _context;

        public WorkoutController(AppDbContext context)
        {
            _context = context;
        }

        private void SetUsernameToViewBag()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
        }

        // Kullanıcının workout listesini göster
        public IActionResult Index()
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var workouts = _context.Workouts
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.Date)
                .ToList();

            var workoutViewModels = new List<WorkoutIndexViewModel>();

            foreach (var workout in workouts)
            {
                var workoutDetails = _context.WorkoutDetails
                    .Where(wd => wd.WorkoutId == workout.Id)
                    .Include(wd => wd.Exercise)
                    .ToList();

                var workoutViewModel = new WorkoutIndexViewModel
                {
                    Id = workout.Id,
                    Date = workout.Date,
                    WorkoutType = workout.WorkoutType,
                    Notes = workout.Notes,
                    WorkoutDetails = workoutDetails.Select(wd => new WorkoutDetailViewModel
                    {
                        Id = wd.Id,
                        SetCount = wd.SetCount,
                        Reps = wd.Reps,
                        Weight = wd.Weight,
                        ExerciseName = wd.Exercise.Name,
                        ExerciseCategory = wd.Exercise.Category
                    }).ToList()
                };

                workoutViewModels.Add(workoutViewModel);
            }

            return View(workoutViewModels);
        }

        // Workout ekleme sayfası
        public IActionResult Create()
        {
            SetUsernameToViewBag();
            var viewModel = new WorkoutCreateViewModel
            {
                Exercises = _context.Exercises.ToList()
            };
            return View(viewModel);
        }

        // Workout ekleme işlemi (POST)
        [HttpPost]
        public IActionResult Create(WorkoutCreateViewModel viewModel)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                var workout = new Workout
                {
                    Date = viewModel.Date,
                    WorkoutType = viewModel.WorkoutType,
                    Notes = viewModel.Notes,
                    UserId = userId.Value
                };
                _context.Workouts.Add(workout);
                _context.SaveChanges();

                // Workout kaydedildikten sonra WorkoutDetail'ları ekle
                foreach (var detail in viewModel.WorkoutDetails)
                {
                    var workoutDetail = new WorkoutDetail
                    {
                        WorkoutId = workout.Id,
                        ExerciseId = detail.ExerciseId,
                        SetCount = detail.SetCount,
                        Reps = detail.Reps,
                        Weight = detail.Weight
                    };
                    _context.WorkoutDetails.Add(workoutDetail);
                }
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            // Hata durumunda tekrar egzersiz listesini doldur
            viewModel.Exercises = _context.Exercises.ToList();
            
            // Eğer WorkoutDetails null ise, boş liste oluştur
            if (viewModel.WorkoutDetails == null)
            {
                viewModel.WorkoutDetails = new List<WorkoutDetailInputModel>();
            }
            
            // Eğer hiç WorkoutDetail yoksa, en az bir tane ekle
            if (viewModel.WorkoutDetails.Count == 0)
            {
                viewModel.WorkoutDetails.Add(new WorkoutDetailInputModel());
            }
            
            return View(viewModel);
        }

        // Workout düzenleme sayfası (GET)
        public IActionResult Edit(int id)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var workout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (workout == null)
                return NotFound();

            return View(workout);
        }

        // Workout düzenleme işlemi (POST)
        [HttpPost]
        public IActionResult Edit(int id, Workout workout)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            if (id != workout.Id)
                return NotFound();

            var existingWorkout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (existingWorkout == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingWorkout.Date = workout.Date;
                existingWorkout.WorkoutType = workout.WorkoutType;
                existingWorkout.Notes = workout.Notes;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(workout);
        }

        // Workout silme sayfası (GET)
        public IActionResult Delete(int id)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var workout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (workout == null)
                return NotFound();

            return View(workout);
        }

        // Workout silme işlemi (POST)
        [HttpPost]
        public IActionResult Delete(int id, Workout workout)
        {
            SetUsernameToViewBag();
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var existingWorkout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (existingWorkout == null)
                return NotFound();

            _context.Workouts.Remove(existingWorkout);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
