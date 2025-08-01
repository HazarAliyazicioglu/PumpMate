using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class Workout
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tarih alanı zorunludur")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Antrenman türü seçiniz")]
        public string WorkoutType { get; set; }

        [Display(Name = "Notlar")]
        public string? Notes { get; set; }  // Nullable yapıldı

        public int UserId { get; set; }
        
        public User? User { get; set; }  // Nullable yapıldı
        
        // Navigation Properties
        public ICollection<WorkoutDetail> WorkoutDetails { get; set; } = new List<WorkoutDetail>();
    }
}
