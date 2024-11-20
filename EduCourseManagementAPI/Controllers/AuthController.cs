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
        public IActionResult Login([FromBody] User request)
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
*/


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationCourseManagement.Services;
using EducationCourseManagement.Models;
using EducationCourseManagement.Data;

namespace EducationCourseManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly SchoolContext _context;

        public AuthController(TokenService tokenService, SchoolContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] User request)
        {
            // Retrieve the user from the database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || user.Password != request.Password) 
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate token based on the user's role
            var token = _tokenService.GenerateToken(user.Role);
            return Ok(new { Token = token });
        }

    }
}



