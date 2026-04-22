using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymAppC.Domain.Entities;

namespace GymAppC.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}