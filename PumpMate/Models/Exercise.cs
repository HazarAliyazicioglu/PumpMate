using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // Eg: Bench Press, Squat

        public string? Category { get; set; } // Eg: Göğüs, Bacak, Sırt (opsiyonel)

        public string? Description { get; set; } // Egzersizin açıklaması (opsiyonel)
    }
}
