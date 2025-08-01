using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required] 
        public string Username { get; set; }
        
        [Required] 
        public string Password { get; set; }

        // Navigations
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();

    }
}
