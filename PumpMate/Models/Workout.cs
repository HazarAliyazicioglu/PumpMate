using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class Workout
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tarih alanı zorunludur")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Antrenman türü seçiniz")]
        [RegularExpression("^(Gym|Home|Cardio|Yoga)$", ErrorMessage = "Geçerli bir antrenman türü seçiniz")]
        [Display(Name = "Antrenman Türü")]
        public string WorkoutType { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string? Notes { get; set; }

        public int UserId { get; set; }
        
        public User? User { get; set; }
        
        // Navigation Properties
        public ICollection<WorkoutDetail> WorkoutDetails { get; set; } = new List<WorkoutDetail>();
    }
}
