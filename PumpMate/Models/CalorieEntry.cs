using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class CalorieEntry
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Tarih alanı zorunludur")]
        public DateTime Date { get; set; } = DateTime.Today;
        
        [Required(ErrorMessage = "Yiyecek/İçecek adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Yiyecek/İçecek adı 2-100 karakter arasında olmalıdır")]
        [Display(Name = "Yiyecek/İçecek Adı")]
        public string FoodName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kalori miktarı zorunludur")]
        [Range(1, 5000, ErrorMessage = "Kalori miktarı 1-5000 arasında olmalıdır")]
        [Display(Name = "Kalori")]
        public int Calories { get; set; }
        
        [Range(1, 10000, ErrorMessage = "Miktar 1-10000 arasında olmalıdır")]
        [Display(Name = "Miktar (gram/porsiyon)")]
        public int? Quantity { get; set; }
        
        [StringLength(500, ErrorMessage = "Notlar en fazla 500 karakter olabilir")]
        [Display(Name = "Notlar")]
        public string? Notes { get; set; }
        
        // Geçici olarak devre dışı - veritabanında bu sütunlar yok
        // [Required(ErrorMessage = "Giriş türü zorunludur")]
        // [RegularExpression("^(Consumed|Burned)$", ErrorMessage = "Giriş türü 'Tüketilen' veya 'Yakılan' olmalıdır")]
        // [Display(Name = "Giriş Türü")]
        // public string EntryType { get; set; } = "Consumed"; // "Consumed" veya "Burned"
        
        // public DateTime EntryTime { get; set; } = DateTime.Now;
        
        public int UserId { get; set; }
        public User? User { get; set; }
    }
} 