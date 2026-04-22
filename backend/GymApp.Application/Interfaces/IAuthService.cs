using GymAppC.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymAppC.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterUserDto dto);

        Task<(bool Success, string Message, AuthResponseDto? Response)> LoginAsync(LoginUserDto dto);
    }
}
