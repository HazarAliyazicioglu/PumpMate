using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PumpMate.Models
{
    public class WorkoutCreateViewModel
    {
        [Required(ErrorMessage = "Tarih alanı zorunludur")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Antrenman türü seçiniz")]
        public string WorkoutType { get; set; }

        public string? Notes { get; set; }

        // Kullanıcının ekleyeceği hareketler ve detayları
        public List<WorkoutDetailInputModel> WorkoutDetails { get; set; } = new List<WorkoutDetailInputModel>();

        // Hareket (egzersiz) listesini doldurmak için
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }

    // Sadece formdan alınacak bilgiler için
    public class WorkoutDetailInputModel
    {
        [Required]
        public int ExerciseId { get; set; }
        [Required]
        public int SetCount { get; set; }
        [Required]
        public int Reps { get; set; }
        public float? Weight { get; set; }
    }
} 