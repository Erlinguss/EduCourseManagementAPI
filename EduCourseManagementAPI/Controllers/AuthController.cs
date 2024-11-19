using Microsoft.AspNetCore.Mvc;
using EducationCourseManagement.Services;

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
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Mock user validation
            if (request.Username == "admin" && request.Password == "admindkit")
            {
                var token = _tokenService.GenerateToken("Admin");
                return Ok(new { Token = token });
            }
            else if (request.Username == "instructor" && request.Password == "instructordkit")
            {
                var token = _tokenService.GenerateToken("Instructor");
                return Ok(new { Token = token });
            }
            else if (request.Username == "student" && request.Password == "studentdkit")
            {
                var token = _tokenService.GenerateToken("Student");
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}

