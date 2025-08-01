using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class WorkoutIndexViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string WorkoutType { get; set; }
        public string? Notes { get; set; }
        public List<WorkoutDetailViewModel> WorkoutDetails { get; set; } = new List<WorkoutDetailViewModel>();
    }

    public class WorkoutDetailViewModel
    {
        public int Id { get; set; }
        public int SetCount { get; set; }
        public int Reps { get; set; }
        public float? Weight { get; set; }
        public string ExerciseName { get; set; }
        public string? ExerciseCategory { get; set; }
    }
} 