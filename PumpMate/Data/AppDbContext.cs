using Microsoft.EntityFrameworkCore;
using PumpMate.Models; // Eğer namespace farklıysa burayı projene göre güncelle

namespace PumpMate.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

                // Veritabanındaki tablolar:
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Workout> Workouts { get; set; } = null!;
        public DbSet<Exercise> Exercises { get; set; } = null!;
        public DbSet<WorkoutDetail> WorkoutDetails { get; set; } = null!;
 
        public DbSet<CalorieEntry> CalorieEntries { get; set; } = null!;
    }
}

