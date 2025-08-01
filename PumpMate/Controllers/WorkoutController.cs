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

        private int? GetCurrentUserId()
        {
            return HttpContext.Session.GetInt32("UserId");
        }

        // Kullanıcının workout listesini göster
        public IActionResult Index()
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Antrenman listesi yüklenirken bir hata oluştu.";
                return RedirectToAction("Index", "Home");
            }
        }

        // Workout ekleme sayfası
        public IActionResult Create()
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                var viewModel = new WorkoutCreateViewModel
                {
                    Exercises = _context.Exercises.ToList(),
                    Date = DateTime.Today
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Sayfa yüklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        // Workout ekleme işlemi (POST)
        [HttpPost]
        public IActionResult Create(WorkoutCreateViewModel viewModel)
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
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
                        if (detail.ExerciseId > 0 && detail.SetCount > 0 && detail.Reps > 0)
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
                    }
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Antrenman başarıyla eklendi!";
                    return RedirectToAction("Index");
                }

                // Hata durumunda exercises'i tekrar yükle
                viewModel.Exercises = _context.Exercises.ToList();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Antrenman eklenirken bir hata oluştu. Lütfen tekrar deneyin.");
                viewModel.Exercises = _context.Exercises.ToList();
                return View(viewModel);
            }
        }

        // Workout düzenleme sayfası
        public IActionResult Edit(int id)
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                var workout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
                if (workout == null)
                {
                    TempData["ErrorMessage"] = "Antrenman bulunamadı.";
                    return RedirectToAction("Index");
                }

                return View(workout);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Antrenman yüklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, Workout workout)
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                var existingWorkout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
                if (existingWorkout == null)
                {
                    TempData["ErrorMessage"] = "Antrenman bulunamadı.";
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    existingWorkout.Date = workout.Date;
                    existingWorkout.WorkoutType = workout.WorkoutType;
                    existingWorkout.Notes = workout.Notes;

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Antrenman başarıyla güncellendi!";
                    return RedirectToAction("Index");
                }

                return View(workout);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Antrenman güncellenirken bir hata oluştu. Lütfen tekrar deneyin.");
                return View(workout);
            }
        }

        // Workout silme sayfası
        public IActionResult Delete(int id)
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                var workout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
                if (workout == null)
                {
                    TempData["ErrorMessage"] = "Antrenman bulunamadı.";
                    return RedirectToAction("Index");
                }

                return View(workout);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Antrenman yüklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Delete(int id, Workout workout)
        {
            try
            {
                SetUsernameToViewBag();
                int? userId = GetCurrentUserId();
                if (userId == null)
                    return RedirectToAction("Login", "Auth");

                var existingWorkout = _context.Workouts.FirstOrDefault(w => w.Id == id && w.UserId == userId);
                if (existingWorkout == null)
                {
                    TempData["ErrorMessage"] = "Antrenman bulunamadı.";
                    return RedirectToAction("Index");
                }

                // İlişkili WorkoutDetail'ları sil
                var workoutDetails = _context.WorkoutDetails.Where(wd => wd.WorkoutId == id);
                _context.WorkoutDetails.RemoveRange(workoutDetails);

                // Workout'u sil
                _context.Workouts.Remove(existingWorkout);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Antrenman başarıyla silindi!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Antrenman silinirken bir hata oluştu.";
                return RedirectToAction("Index");
            }
        }
    }
}
