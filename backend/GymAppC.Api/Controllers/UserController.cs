using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymAppC.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = User.FindFirst("id")?.Value;
            return Ok(new
            {

                message = "Du är inloggad 🎉",

                userId = userId

            });
        }
    }
}