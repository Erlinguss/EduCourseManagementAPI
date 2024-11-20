using Microsoft.AspNetCore.Mvc;
using EducationCourseManagement.Services;
using EducationCourseManagement.Models;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login request)
        {
            // Mock user validation with role
            if (request.Username == "PeterAdmin" && request.Password == "admindkit" && request.Role == "Admin")
            {
                var token = _tokenService.GenerateToken(request.Role);
                return Ok(new { Token = token });
            }
            else if (request.Username == "JohnInstructor" && request.Password == "instructordkit" && request.Role == "Instructor")
            {
                var token = _tokenService.GenerateToken(request.Role);
                return Ok(new { Token = token });
            }
            else if (request.Username == "DavidStudent" && request.Password == "studentdkit" && request.Role == "Student")
            {
                var token = _tokenService.GenerateToken(request.Role);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials or role.");
        }
    }
}
