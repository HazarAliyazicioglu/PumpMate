namespace PumpMate.Models
{
    public class CalorieViewModel
    {
        public List<CalorieEntry> CalorieEntries { get; set; } = new List<CalorieEntry>();
        public CalorieEntry NewEntry { get; set; } = new CalorieEntry();
        
        // Günlük istatistikler
        public int TotalCaloriesConsumed { get; set; }
        public int TotalCaloriesBurned { get; set; }
        public int CalorieDeficit { get; set; }
        
        // Haftalık istatistikler
        public int WeeklyCaloriesConsumed { get; set; }
        public int WeeklyCaloriesBurned { get; set; }
        public int WeeklyCalorieDeficit { get; set; }
        
        // Antrenman kalori hesaplama
        public List<WorkoutCalorieInfo> WorkoutCalories { get; set; } = new List<WorkoutCalorieInfo>();
    }
    
    public class WorkoutCalorieInfo
    {
        public DateTime Date { get; set; }
        public string WorkoutType { get; set; } = string.Empty;
        public int TotalCaloriesBurned { get; set; }
        public List<ExerciseCalorieInfo> Exercises { get; set; } = new List<ExerciseCalorieInfo>();
    }
    
    public class ExerciseCalorieInfo
    {
        public string ExerciseName { get; set; } = string.Empty;
        public int SetCount { get; set; }
        public int Reps { get; set; }
        public float? Weight { get; set; }
        public int CaloriesBurned { get; set; }
    }
} 