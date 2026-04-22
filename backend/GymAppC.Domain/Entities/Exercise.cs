using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymAppC.Domain.Entities
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Sets { get; set; }
        public int Reps { get; set; }

        public int WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;
    }
}
