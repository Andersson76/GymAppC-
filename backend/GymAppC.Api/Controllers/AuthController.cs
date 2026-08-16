using GymAppC.Application.Dtos;
using GymAppC.Application.Features.Auth.Commands.Register;
using GymAppC.Application.Features.Auth.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GymAppC.Api.Controllers;

[AllowAnonymous]
[EnableRateLimiting("authentication")]
[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new RegisterUserCommand(dto.Name, dto.Email, dto.Password),
            cancellationToken);

        if (!result.Succeeded)
        {
            return BadRequest(new { message = result.Error });
        }

        return Ok(new { message = result.Value });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new LoginUserQuery(dto.Email, dto.Password),
            cancellationToken);

        if (!result.Succeeded || result.Value is null)
        {
            return Unauthorized(new { message = result.Error });
        }

        return Ok(result.Value);
    }
}
