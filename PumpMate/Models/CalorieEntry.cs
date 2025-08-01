using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class CalorieEntry
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tarih alanı zorunludur")]
        public DateTime Date { get; set; } = DateTime.Today;
        
        [Required(ErrorMessage = "Yemek adı zorunludur")]
        [Display(Name = "Yemek Adı")]
        public string FoodName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kalori miktarı zorunludur")]
        [Range(1, 5000, ErrorMessage = "Kalori miktarı 1-5000 arasında olmalıdır")]
        [Display(Name = "Kalori")]
        public int Calories { get; set; }
        
        [Display(Name = "Miktar (gram/porsiyon)")]
        public int? Quantity { get; set; }
        
        [Display(Name = "Notlar")]
        public string? Notes { get; set; }
        
        public int UserId { get; set; }
        public User? User { get; set; }
    }
} 