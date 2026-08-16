using System.Security.Claims;
using GymAppC.Application.DTOs.Workouts;
using GymAppC.Application.Features.Workouts.Commands.CreateWorkout;
using GymAppC.Application.Features.Workouts.Commands.DeleteWorkout;
using GymAppC.Application.Features.Workouts.Commands.UpdateWorkout;
using GymAppC.Application.Features.Workouts.Queries.GetWorkoutById;
using GymAppC.Application.Features.Workouts.Queries.GetWorkouts;
using GymAppC.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppC.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public sealed class WorkoutsController : ControllerBase
{
    private readonly ISender _sender;

    public WorkoutsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyWorkouts(CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var workouts = await _sender.Send(
            new GetWorkoutsQuery(userId.Value),
            cancellationToken);

        return Ok(workouts);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetWorkoutById(
        int id,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var workout = await _sender.Send(
            new GetWorkoutByIdQuery(id, userId.Value),
            cancellationToken);

        return workout is null ? NotFound() : Ok(workout);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkout(
        [FromBody] CreateWorkoutDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var createdWorkout = await _sender.Send(
            new CreateWorkoutCommand(dto.Title, dto.Date, dto.Notes, userId.Value),
            cancellationToken);

        return CreatedAtAction(
            nameof(GetWorkoutById),
            new { id = createdWorkout.Id },
            createdWorkout);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateWorkout(
        int id,
        [FromBody] UpdateWorkoutDto dto,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var updated = await _sender.Send(
            new UpdateWorkoutCommand(
                id,
                dto.Title,
                dto.Date,
                dto.Notes,
                userId.Value),
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteWorkout(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _sender.Send(
            new DeleteWorkoutCommand(id),
            cancellationToken);

        return deleted ? NoContent() : NotFound();
    }

    private int? GetUserId()
    {
        return int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var userId)
            ? userId
            : null;
    }
}
