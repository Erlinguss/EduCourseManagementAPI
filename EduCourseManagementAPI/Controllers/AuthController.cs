/*using Microsoft.AspNetCore.Mvc;
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
*/

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
