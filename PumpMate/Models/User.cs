using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Kullanıcı adı 3-50 karakter arasında olmalıdır.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir.")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre gereklidir.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = string.Empty;

        // Navigations
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<CalorieEntry> CalorieEntries { get; set; } = new List<CalorieEntry>();
    }
}
