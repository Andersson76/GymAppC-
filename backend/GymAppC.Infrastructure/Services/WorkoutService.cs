using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Interfaces;
using GymAppC.Domain.Entities;
using GymAppC.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymAppC.Infrastructure.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly AppDbContext _context;
        public WorkoutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<WorkoutDto>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Workouts
                .Where(w => w.UserId == userId)
                .Select(w => new WorkoutDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Date = w.Date,
                    Notes = w.Notes,
                    UserId = w.UserId
                })
                .ToListAsync();
        }
        public async Task<WorkoutDto?> GetByIdAsync(int id, int userId)
        {
            return await _context.Workouts
                .Where(w => w.Id == id && w.UserId == userId)
                .Select(w => new WorkoutDto
                {
                    Id = w.Id,
                    Title = w.Title,
                    Date = w.Date,
                    Notes = w.Notes,
                    UserId = w.UserId
                })
                .FirstOrDefaultAsync();
        }
        public async Task<WorkoutDto> CreateAsync(CreateWorkoutDto dto, int userId)
        {
            var workout = new Workout
            {
                Title = dto.Title,
                Date = dto.Date,
                Notes = dto.Notes,
                UserId = userId
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return new WorkoutDto
            {
                Id = workout.Id,
                Title = workout.Title,
                Date = workout.Date,
                Notes = workout.Notes,
                UserId = workout.UserId
            };
        }
        public async Task<bool> UpdateAsync(int id, UpdateWorkoutDto dto, int userId)
        {
            var workout = await _context.Workouts
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (workout == null)
                return false;

            workout.Title = dto.Title;
            workout.Date = dto.Date;
            workout.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var workout = await _context.Workouts
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (workout == null)
                return false;

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
