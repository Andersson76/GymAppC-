using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymAppC.Application.DTOs.Workouts;

namespace GymAppC.Application.Interfaces
{
    public interface IWorkoutService
    {
        Task<IEnumerable<WorkoutDto>> GetAllByUserIdAsync(int userId);
        Task<WorkoutDto?> GetByIdAsync(int id, int userId);
        Task<WorkoutDto> CreateAsync(CreateWorkoutDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdateWorkoutDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
