using System.Security.Claims;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppC.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : ControllerBase
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutsController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWorkouts()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var workouts = await _workoutService.GetAllByUserIdAsync(userId.Value);
            return Ok(workouts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkoutById(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var workout = await _workoutService.GetByIdAsync(id, userId.Value);

            if (workout == null)
                return NotFound();

            return Ok(workout);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkout(CreateWorkoutDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var createdWorkout = await _workoutService.CreateAsync(dto, userId.Value);

            return CreatedAtAction(nameof(GetWorkoutById), new { id = createdWorkout.Id }, createdWorkout);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkout(int id, UpdateWorkoutDto dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var updated = await _workoutService.UpdateAsync(id, dto, userId.Value);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            var deleted = await _workoutService.DeleteAsync(id, userId.Value);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out var userId))
                return userId;

            return null;
        }
    }
}