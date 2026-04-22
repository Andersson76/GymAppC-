using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GymAppC.Application.DTOs.Workouts

{

    public class UpdateWorkoutDto

    {

        [Required]

        [MaxLength(100)]

        public string Title { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [MaxLength(500)]

        public string? Notes { get; set; }

    }

}